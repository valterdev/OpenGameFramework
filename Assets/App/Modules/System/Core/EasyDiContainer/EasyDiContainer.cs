using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace OpenGameFramework.DI
{
    public class EasyDiContainer
    {
        private readonly Dictionary<Type, Type> _bindings = new();
        private readonly DependencyGraph _dependencyGraph = new();
        private readonly Dictionary<Type, object> _services = new();
        
        public void PreInit()
        {
            List<InjectRelation> injectInfo = new();
            
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                Type[] classTypes = assembly
                    .GetTypes()
                    .Where(x => x.GetCustomAttributes().OfType<InjectAttribute>().Any())
                    .ToArray();

                if (classTypes.Length <= 0) continue;

                List<InjectRelation> info = new();
                
                foreach (var curType in classTypes)
                {
                    info.Add(new InjectRelation()
                    {
                        Interface = curType.GetCustomAttribute<InjectAttribute>().InterfaceType,
                        RealClass = curType
                    });
                    
                    injectInfo.Add(info.Last());
                }

                foreach (var curInject in info)
                {
                    RegisterService(curInject.Interface, curInject.RealClass);
                }
            }
            
            InstantiateAllServices(injectInfo);
        }

        public void Init()
        {
            foreach (var service in _services)
            {
                (service.Value as IServiceDi)?.Init();
            }
        }

        private void InstantiateAllServices(List<InjectRelation> injectInfos)
        {
            GenerateDependenciesGraph(injectInfos);

            if (_dependencyGraph.HaveCycledDependencies())
            {
                Debug.LogError("Composition error: Cycled dependencies");
                return;
            }

            _dependencyGraph.ResolveDependencies(InitService);
        }

        private void GenerateDependenciesGraph(List<InjectRelation> injectInfos)
        {
            foreach (var curInject in injectInfos)
            {
                if (curInject.RealClass.GetConstructors().First().GetParameters().Length > 0)
                {
                    var paramTypes = curInject.RealClass.GetConstructors()
                        .First()
                        .GetParameters()
                        .Select(x => x.ParameterType);

                    _dependencyGraph.AddNodes(curInject, paramTypes
                        .Select(interfaceType => new InjectRelation()
                        {
                            Interface = interfaceType,
                            RealClass = _bindings[interfaceType]
                        }).ToArray()
                    );
                }
                else
                {
                    // add root nodes (Where constructor has no parameters)
                    _dependencyGraph.AddNode(curInject);
                }
            }
        }

        private void RegisterService(Type interfaceType, Type classType)
        {
            if (_bindings.ContainsKey(interfaceType)) return;
            _bindings.Add(interfaceType, classType);
        }

        private IServiceDi InitService(Type curType)
        {
            if (curType.GetConstructors()
                    .First()
                    .GetParameters().Length > 0)
            {
                var constructorParameters =
                    curType.GetConstructors()
                        .First()
                        .GetParameters()
                        .Select(x => x.ParameterType).ToArray();

                var constructorRealParameters = constructorParameters
                    .Select(parameterType => _bindings[parameterType]).ToArray();
                
                var constructor = curType.GetConstructorAccepting(constructorRealParameters, false);
                var instancesForParameters = constructorRealParameters
                    .Select(curType => _services[curType]).ToArray();
                
                var instance = constructor.Invoke(instancesForParameters);
                AddSingleInstance(curType, instance);
                
                return instance as IServiceDi;
            }
            
            // invoke the first public constructor with no parameters.
            var instanceNoParams = curType.GetConstructors().First().Invoke(new object[] { });
            AddSingleInstance(curType, instanceNoParams);

            return instanceNoParams as IServiceDi;
        }

        private void AddSingleInstance(Type instanceType, object instance)
        {
            if (_services.ContainsKey(instanceType)) return;
            
            _services.Add(instanceType, instance);
        }

        // --- How it works on Zenject: --------------------
        // var newConstructMethod = TryCreateFactoryMethodCompiledLambdaExpression(
        //     curType, 
        //     curType.GetConstructors().First());
        //
        // return (T)newConstructMethod.Invoke();
        // --------------------------------------------------
        // --- How it works on Zenject: (Faster than Activator)-------------
        static Func<object[], object> TryCreateFactoryMethodCompiledLambdaExpression(
            Type type, ConstructorInfo constructor)
        {
#if NET_4_6 && !ENABLE_IL2CPP

            if (type.ContainsGenericParameters)
            {
                return null;
            }

            ParameterExpression param = Expression.Parameter(typeof(object[]));

            if (constructor == null)
            {
                return Expression.Lambda<Func<object[], object>>(
                    Expression.Convert(
                        Expression.New(type), typeof(object)), param).Compile();
            }

            ParameterInfo[] par = constructor.GetParameters();
            Expression[] args = new Expression[par.Length];

            for (int i = 0; i != par.Length; ++i)
            {
                Debug.Log(par[i].ParameterType);
                args[i] = Expression.Convert(
                    Expression.ArrayIndex(
                        param, Expression.Constant(i)), par[i].ParameterType);
            }

            return Expression.Lambda<Func<object[], object>>(
                Expression.Convert(
                    Expression.New(constructor, args), typeof(object)), param).Compile();
#else
            return null;
#endif
        }
        // --------------------------------------------------
    }
}