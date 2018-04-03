using CodeTool.Models;
using hundsun.t2sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CodeTool.Util;

namespace CodeTool.Service
{
    public class CodeToolService
    {
        private List<SendFunctionDto> _sendFunctionDtoList;
        private ShowList _showlist;
        private CT2Configinterface _config = null;
        public CT2Connection Conn = null;
        private Callbacktest _callback = null;

        public CodeToolService()
        {
            _sendFunctionDtoList = new List<SendFunctionDto>();
        }

        private void SetSendFunctionStrs(string strs)
        {
            if (strs.Contains(","))
            {
                var valueList = strs.Split(',');
                foreach (var item in valueList)
                {
                    if (item.Contains("="))
                    {
                        var valueTempList = item.Split('=');
                        var firstOrDefault = ReadFile.DictionaryList?.FirstOrDefault(s => s.Name == valueTempList[0]);
                        if (firstOrDefault!=null)
                        {
                            var sendFunctionDto = new SendFunctionDto
                            {
                                Name = valueTempList[0],
                                Value = valueTempList[1],
                                Type = firstOrDefault.Type,
                            };
                            _sendFunctionDtoList.Add(sendFunctionDto);
                        }
                    }
                }
            }

            var aa = _sendFunctionDtoList;
        }

        /// <summary>
        /// 功能号发送
        /// </summary>
        public void FuntionSend(int functionId, string functionIdGenerationStr, Action<ShowList> callback)
        {
            var flag = Connect();
            if (flag == 0)
            {
                SetSendFunctionStrs(functionIdGenerationStr);
                SendFunctionAction(functionId,functionIdGenerationStr, callback);
            }
            else
            {
                MessageBox.Show("连接失败！");
            }
        }

        public void SendFunctionAction(int functionId,string functionIdGenerationStr, Action<ShowList> callback)
        {
            CT2Esbmsg t2Message = new CT2Esbmsg();//构造消息
            t2Message.GetItem(CT2tag_def.TAG_FUNCTION_ID).SetInt(functionId, 0);//设置功能号
            t2Message.GetItem(CT2tag_def.TAG_PACKET_TYPE).SetInt(0, 0); ;//设置消息类型为请求
            //打包请求报文
            CT2Packer packer = new CT2Packer(3);
            sbyte strType = Convert.ToSByte('S');
            sbyte intType = Convert.ToSByte('I');
            sbyte charType = Convert.ToSByte('C');
            packer.BeginPack();
            //插件编号
            //管理功能号

            foreach (var item in _sendFunctionDtoList)
            {
                if (item.Type == "int")
                {
                    packer.AddField(item.Name, intType, 255, 4);
                }
                else if(item.Type == "string")
                {
                    packer.AddField(item.Name, strType, 255, 4);
                }
            }

            foreach (var item in _sendFunctionDtoList)
            {
                if (item.Type == "int")
                {
                    packer.AddInt(Convert.ToInt32(item.Value));
                }
                else if (item.Type == "string")
                {
                    packer.AddStr(item.Value);
                }
            }

            packer.EndPack();

            unsafe
            {
                t2Message.GetItem(CT2tag_def.TAG_MESSAGE_BODY).SetRawData(packer.GetPackBuf(), packer.GetPackLen());
                int iMsgLen = 0;
                void* lpData = t2Message.GetBuffer(&iMsgLen);
                int iRet = Conn.Send(lpData, iMsgLen, 0);
                if (iRet < 0)
                {
                    MessageBox.Show(Conn.GetErrorMsg(iRet));
                }
                else
                {
                    void* lpRecvData = null;
                    int iRecvLen = 0;
                    iRet = Conn.Receive(iRet, &lpRecvData, &iRecvLen, 5000, 0);
                    if (iRet == 0)
                    {
                        CT2Esbmsg ansMessage = new CT2Esbmsg();//构造消息
                        int iParseError = ansMessage.SetBuffer(lpRecvData, iRecvLen);//解析消息
                        if (iParseError != 0)
                        {
                            MessageBox.Show("同步接收业务错误：解析消息失败\n");
                        }
                        else
                        {
                            int iRetCode = ansMessage.GetItem(CT2tag_def.TAG_RETURN_CODE).GetInt(0); //获取返回码
                            int iErrorCode = ansMessage.GetItem(CT2tag_def.TAG_ERROR_NO).GetInt(0);//获取错误码
                            if (iErrorCode != 0)
                            {
                                MessageBox.Show("同步接收出错：" + ansMessage.GetItem(CT2tag_def.TAG_ERROR_NO).GetString(0) +
                                            ansMessage.GetItem(CT2tag_def.TAG_ERROR_INFO).GetString(0));
                            }
                            else
                            {
                                CT2UnPacker unpacker = null;
                                unsafe
                                {
                                    int iLen = 0;
                                    void* lpdata = ansMessage.GetItem(CT2tag_def.TAG_MESSAGE_BODY).GetRawData(&iLen, 0);
                                    unpacker = new CT2UnPacker(lpdata, (uint)iLen);
                                }
                                //返回业务错误
                                if (iRetCode != 0)
                                {
                                    MessageBox.Show("同步接收业务出错：\n");
                                    UnPack(unpacker);
                                }
                                //正常业务返回
                                else
                                {
                                    UnPack(unpacker);
                                    callback?.Invoke(_showlist);
                                }

                                unpacker.Dispose();
                            }

                        }
                        ansMessage.Dispose();
                    }
                    else
                    {
                        Conn.GetErrorMsg(iRet);
                    }
                }
            }
            t2Message.Dispose();
            packer.Dispose();
        }

        public void UnPack(CT2UnPacker lpUnPack)
        {
            var flag = 0;
            var count = lpUnPack.GetRowCount();
            _showlist = new ShowList();
            _showlist.ValueList = new List<string>[count];
            _showlist.NameList = new List<string>();

            for (int i = 0; i < count; i++)
            {
                _showlist.ValueList[i] = new List<string>();
            }


            while (lpUnPack.IsEOF() != 1)
            {
                for (int j = 0; j < lpUnPack.GetColCount(); j++)
                {
                    var colName = lpUnPack.GetColName(j);
                    sbyte colType = lpUnPack.GetColType(j);
                    if (colType != 'R')
                    {
                        var colValue = lpUnPack.GetStrByIndex(j);
                        if (flag == 0)
                        {
                            _showlist.NameList.Add(colName);
                        }
                        _showlist.ValueList[flag].Add(colValue);
                    }
                }

                lpUnPack.Next();
                flag++;
            }
            var aa = _showlist;
        }

        public int Connect()
        {
            _config = new CT2Configinterface();
            _config.Load("t2sdk.ini");
            Conn = new CT2Connection(_config);
            _callback = new Callbacktest();
            Conn.Create(_callback);
            var iret = Conn.Connect(5000);
            return iret;
        }
    }

    public unsafe class Callbacktest : CT2CallbackInterface
    {
        public override void OnClose(CT2Connection lpConnection)
        {
            System.Console.WriteLine(@"连接成功！");
        }

        public override void OnConnect(CT2Connection lpConnection)
        {
            System.Console.WriteLine(@"连接成功！");
        }

        public override unsafe void OnReceived(CT2Connection lpConnection, int hSend, void* lpData, int nLength)
        {
            System.Console.WriteLine(@"连接成功！");
        }

        public override void OnRegister(CT2Connection lpConnection)
        {
            System.Console.WriteLine(@"OnRegister");
        }

        public override void OnSafeConnect(CT2Connection lpConnection)
        {
            System.Console.WriteLine(@"OnSafeConnect");
        }

        public override unsafe void OnSent(CT2Connection lpConnection, int hSend, void* lpData, int nLength, int nQueuingData)
        {
            System.Console.WriteLine(@"连接成功！");
        }
    }
}
