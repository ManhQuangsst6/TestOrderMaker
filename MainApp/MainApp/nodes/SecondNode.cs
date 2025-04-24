using MainApp.Nodes;
using System.Arctec.Ar1000k.DataBase;

namespace TestOrderMaker
{
    class SecondNode : NodeBase
    {
        #region メンバ変数
        private MedicalItemListRecord medical_item_list_record;
        #endregion

        #region コンストラクタ
        public SecondNode(MedicalItemListRecord medical_item_list_record)
        {
            this.medical_item_list_record = medical_item_list_record;
            this.node = new TreeNode(medical_item_list_record.ItemName);
            this.node.Checked = true;
            this.node.Tag = medical_item_list_record.MedicalCheckNo + "/" + medical_item_list_record.ItemNo;
        }
        #endregion
    }
}
