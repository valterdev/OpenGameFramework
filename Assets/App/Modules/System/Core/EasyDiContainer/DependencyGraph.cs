using System.Collections.Generic;
using System;
using UnityEngine;

namespace OpenGameFramework.DI
{
    public class DependencyGraph
    {
        private Dictionary<InjectRelation, DependencyNode> _nodes = new();
        private HashSet<InjectRelation> _resolvedInjections = new();
        public void AddNode(InjectRelation node)
        {
            if (!_nodes.ContainsKey(node))
            {
                _nodes.Add(node, new DependencyNode(){ Node = node });
            }
        }

        public void AddNodes(InjectRelation node, InjectRelation[] dependencies)
        {
            foreach (var dependency in dependencies)
            {
                if (!_nodes.ContainsKey(node))
                {
                    _nodes.Add(node, new DependencyNode(){ Node = node });
                }
                
                if (!_nodes.ContainsKey(dependency))
                {
                    LinkedList<InjectRelation> childs = new(); 
                    childs.AddLast(node);
                    
                    _nodes.Add(dependency, new DependencyNode()
                    {
                        Node = dependency, 
                        Childs = childs
                    });
                }
                else
                {
                    if (!_nodes[dependency].Childs.Contains(node))
                    {
                        _nodes[dependency].Childs.AddLast(node);
                    }
                }
            }
        }

        public bool HaveCycledDependencies()
        {
            foreach (var node in _nodes)
            {
                if (node.Value.Childs == null) continue;
                
                foreach (var dependency in node.Value.Childs)
                {
                    if (node.Key == dependency) return true;
                }
            }

            return false;
        }
        
        public void ResolveDependencies(Func<Type, IServiceDi> resolveAction)
        {
            _resolvedInjections.Clear();
            
            foreach (var rootNode in _nodes)
            {
                if (rootNode.Value.Childs == null)
                {
                    DepthFirstSearch(rootNode.Value, resolveAction);
                }
            }
        }

        private void DepthFirstSearch(DependencyNode root, Func<Type, IServiceDi> resolveAction)
        {
            if (_resolvedInjections.Add(root.Node))
            {
                resolveAction(root.Node.RealClass).PreInit();
            }

            if (root.Childs == null) return;
            
            foreach (var node in root.Childs)
            {
                if (_resolvedInjections.Add(node))
                {
                    resolveAction(node.RealClass).PreInit();

                    if (_nodes[node].Childs != null)
                    {
                        DepthFirstSearch(_nodes[node], resolveAction);
                    }
                }
            }
        }
    }

    public class DependencyNode
    {
        public InjectRelation Node;
        public LinkedList<InjectRelation> Childs;
    }
}