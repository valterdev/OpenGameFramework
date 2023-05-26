using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace OpenGameFramework.DI
{
    public class DependencyGraph
    {
        private Dictionary<InjectRelation, DependencyNode> _nodes = new();
        Dictionary<int, HashSet<InjectRelation>> _layers = new();
        private HashSet<InjectRelation> _resolvedInjections = new();

        public void AddNode(InjectRelation node)
        {
            if (!_nodes.ContainsKey(node))
            {
                _nodes.Add(node, new DependencyNode() { Node = node });
            }
        }

        public void AddNodes(InjectRelation node, InjectRelation[] dependencies)
        {
            if (!_nodes.ContainsKey(node))
            {
                _nodes.Add(node, new DependencyNode() { Node = node });
            }

            _nodes[node].Parent = true;

            foreach (var dependency in dependencies)
            {
                if (!_nodes.ContainsKey(dependency))
                {
                    _nodes.Add(dependency, new DependencyNode() { Node = dependency });
                }

                _nodes[dependency].Childs ??= new LinkedList<DependencyNode>();

                if (!_nodes[dependency].Childs.Select(x => x.Node).Contains(node))
                {
                    _nodes[dependency].Childs.AddLast(new DependencyNode() { Node = node });
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
                    if (node.Key.Equals(dependency.Node)) return true;
                }
            }

            return false;
        }

        public void ResolveDependencies(Func<Type, IServiceDi> resolveAction)
        {
            _resolvedInjections.Clear();
            BreadhFirstSearch(resolveAction);
        }

        private void BreadhFirstSearch(Func<Type, IServiceDi> resolveAction)
        {
            HashSet<InjectRelation> itemCovered = new();
            Queue<InjectRelation> queue = new();
            List<InjectRelation> initList = new();

            foreach (var rootNode in _nodes
                         .Where(x => !x.Value.Parent))
            {
                queue.Enqueue(rootNode.Key);
            }

            while (queue.Count > 0)
            {
                var element = queue.Dequeue();
                if (itemCovered.Contains(element))
                {
                    continue;
                }

                itemCovered.Add(element);
                
                initList.Add(element);
                _nodes.TryGetValue(element, out var neighbours);

                if (neighbours?.Childs == null)
                    continue;

                foreach (var item1 in neighbours.Childs)
                {
                    queue.Enqueue(item1.Node);
                }
            }
            
            foreach (var service in initList)
            {
                resolveAction(service.RealClass).PreInit();
            }
        }
    }

    public class DependencyNode
    {
        public InjectRelation Node { get; set; }
        public bool Parent { get; set; } = false;
        public LinkedList<DependencyNode> Childs { get; set; }
    }
}