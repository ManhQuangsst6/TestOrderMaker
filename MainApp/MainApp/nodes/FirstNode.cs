using MainApp.Nodes;
using System.Arctec.Ar1000k.DataBase;
using System.Collections.Generic;

namespace TestOrderMaker
{
    class FirstNode : NodeBase
    {
        #region メンバ変数
        private SecondNode[] nodes;
        private MedicalCheckList medical_check_list;
        #endregion

        #region コンストラクタ
        public FirstNode(MedicalCheckList medical_check_list, List<MedicalItemListRecord> medical_item_list_records)
        {
            this.medical_check_list = medical_check_list;
            nodes = new SecondNode[medical_item_list_records.Count];
            this.node = new TreeNode(medical_check_list.MedicalCheckName);
            for (int i = 0; i < medical_item_list_records.Count; i++)
            {
                nodes[i] = new SecondNode(medical_item_list_records[i]);
                this.node.Nodes.Add(nodes[i].Node);
            }
            this.node.Checked = true;
            this.node.Tag = medical_check_list.MedicalCheckNo;
        }
        #endregion

        #region SecondNode取得
        public SecondNode getSecondNode(string item_no)
        {
            foreach (SecondNode node in nodes)
            {
                string[] element = node.Tag.Split('/');
                if (element[1] == item_no)
                {
                    return node;
                }
            }
            return null;
        }
        #endregion

        #region 子Nodeが全て未チェックか
        public bool isAllNoChecked()
        {
            foreach (SecondNode node in nodes)
            {
                if (node.NodeChecked)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 子ノードをすべてチェック
        public void allChecked(bool bool_checked)
        {
            foreach (SecondNode second_node in nodes)
            {
                second_node.NodeChecked = bool_checked;
            }
        }
        #endregion

        #region すべて展開
        public void allExpand()
        {
            // this.node.ExpandAll();
        }
        #endregion

        #region 折りたたむ
        public void collapse()
        {
            //this.node.Collapse();
        }
        #endregion

        #region プロパティ
        public SecondNode[] SecondNodes
        {
            get
            {
                return nodes;
            }
        }
        public string MedicalCheclNo
        {
            get
            {
                return medical_check_list.MedicalCheckNo;
            }
        }
        #endregion
    }
}
