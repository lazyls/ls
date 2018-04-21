using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ls.Common.Generic
{
    public class TreeNode<TValue>
    {
        public TValue Value { get; set; }
        [ScriptIgnore]
        public TreeNode<TValue> Parent { get; set; }
        public List<TreeNode<TValue>> Children { get; set; }

        public bool IsLeaf
        {
            get
            {
                return Children == null || Children.Count == 0;
            }
        }

        public bool IsRoot
        {
            get
            {
                return Parent == null;
            }
        }

        public int Depth
        {
            get
            {
                return GetAncestors().Count;
            }
        }

        [ScriptIgnore]
        public TreeNode<TValue> Root
        {
            get
            {
                var root = this;
                while (!root.IsRoot)
                    root = root.Parent;
                return root;
            }
        }

        [ScriptIgnore]
        public List<TreeNode<TValue>> Descendants
        {
            get
            {
                return GetDescendants();
            }
        }
        [ScriptIgnore]
        public List<TreeNode<TValue>> DescendantsAndSelf
        {
            get
            {
                var rv = GetDescendants();
                rv.Insert(0, this);
                return rv;
            }
        }
        [ScriptIgnore]
        public List<TreeNode<TValue>> Ancestors
        {
            get
            {
                return GetAncestors();
            }
        }
        [ScriptIgnore]
        public List<TreeNode<TValue>> AncestorsAndSelf
        {
            get
            {
                var rv = GetAncestors();
                rv.Add(this);
                return rv;
            }
        }

        List<TreeNode<TValue>> GetDescendants()
        {
            var rv = new List<TreeNode<TValue>>();
            if (!IsLeaf)
            {
                rv.AddRange(Children);
                foreach (var v in Children)
                {
                    rv.AddRange(v.GetDescendants());
                }
            }
            return rv;
        }

        List<TreeNode<TValue>> GetAncestors()
        {
            var rv = new List<TreeNode<TValue>>();
            if (!IsRoot)
            {
                rv.Add(Parent);
                rv.AddRange(Parent.GetAncestors());
            }
            return rv;
        }

        public static List<TreeNode<TValue>> BuildForest<TKey>(List<TValue> list, Func<TValue, TKey> innerKeySelector, Func<TValue, TKey> outerKeySelector)
        {
            List<TreeNode<TValue>> rv = new List<TreeNode<TValue>>();

            var node_list = (list ?? new List<TValue>()).Select(i => new TreeNode<TValue>(i)).ToList();
            foreach (var v in node_list)
            {
                bool flag = false;
                foreach (var vv in node_list)
                {
                    if (outerKeySelector(v.Value).Equals(innerKeySelector(vv.Value)))
                    {
                        flag = true;
                        v.Parent = vv;
                        vv.Children.Add(v);
                        break;
                    }
                }
                if (!flag)
                    rv.Add(v);
            }
            return rv;
        }

        public static TreeNode<TValue> BuildTree<TKey>(List<TValue> list, Func<TValue, TKey> innerKeySelector, Func<TValue, TKey> outerKeySelector)
        {
            var forest = BuildForest(list, innerKeySelector, outerKeySelector);
            return forest.SingleOrDefault();
        }

        public void FilterDescendants(Func<TreeNode<TValue>, bool> predicate)
        {
            if (!IsLeaf)
                Children.Filter(predicate);
        }

        public dynamic Format(Func<TreeNode<TValue>, dynamic> format)
        {
            var val = format(this);
            if (!IsLeaf)
            {
                val.children = new List<dynamic>();
                Children.ForEach(i => val.children.Add(i.Format(format)));
            }
            return val;
        }

        #region ctor

        public TreeNode()
        {
            Children = new List<TreeNode<TValue>>();
        }

        public TreeNode(TValue value)
            : this()
        {
            Value = value;
        }

        #endregion
    }

    public static class TreeNodeExtensions
    {
        public static void Filter<TValue>(this List<TreeNode<TValue>> nodes, Func<TreeNode<TValue>, bool> predicate)
        {
            List<TreeNode<TValue>> leafs;
            do
            {
                leafs = nodes.SelectMany(a => a.DescendantsAndSelf.Where(b => b.IsLeaf && !predicate(b))).ToList();
                foreach (var v in leafs)
                {
                    if (nodes.Contains(v))
                        nodes.Remove(v);
                    else
                        v.Parent.Children.Remove(v);
                }
            } while (leafs.Count > 0);
        }
    }
}
