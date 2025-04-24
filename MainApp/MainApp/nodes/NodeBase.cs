using MainApp.Nodes;

namespace TestOrderMaker
{
    class NodeBase
    {
        #region メンバ変数
        protected TreeNode node;
        #endregion

        #region プロパティ
        public TreeNode Node
        {
            get
            {
                return node;
            }
        }
        public string Tag
        {
            get
            {
                return node.Tag.ToString();
            }
        }
        public bool NodeChecked
        {
            get
            {
                return this.node.Checked;
            }
            set
            {
                // 必要があるときだけ値を更新する
                // memo: でないと、クリックイベントハンドラが無限ループするので注意..
                if (this.node.Checked != value)
                {
                    this.node.Checked = value;
                }
            }
        }
        #endregion
    }
}
