namespace CodeTool.Service
{
    public class CodeToolService
    {
        /// <summary>
        /// 功能号发送
        /// </summary>
        public void FuntionSend(string fundAccount, Action<ShowList> callback)
        {
            var flag = Connect();
            if (flag == 0)
            {
                FundQryAction(fundAccount, callback);
            }
            else
            {
                MessageBox.Show("连接失败！");
            }
        }
    }
}
