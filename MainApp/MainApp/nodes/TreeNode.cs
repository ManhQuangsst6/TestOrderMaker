using System.Collections.Generic;

namespace MainApp.Nodes
{
    public class TreeNode
    {
        public string ItemName { get; set; }
        public string Tag { get; set; }
        public bool Checked { get; set; }
        public TreeNode(string itemName)
        {
            ItemName = itemName;
        }
        public List<TreeNode> Nodes { get; set; } = new List<TreeNode>();
    }
}
