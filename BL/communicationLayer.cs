using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
namespace BL
{
    class communicationLayer
    {
        string UserName;
        Socket sender;
        NetworkStream stream;
        TcpClient client;
        public communicationLayer()
        {

            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 8080 on the local computer.  
              /*  IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 8080);*/

                client = new TcpClient("10.0.0.7", 8080);  //("127.0.0.1", 8080);
                stream = client.GetStream();
                // Create a TCP/IP  socket.  
                /*sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(remoteEP);
                Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());*/
                ThreadStart childref = new ThreadStart(StartClient);
                Thread childThread = new Thread(childref);
                childThread.Start();
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }

            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }

        public void StartClient()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[4096];
            string recived = "";
            try
            {
                while (true)
                {

                    using (StreamReader reader = new StreamReader(stream, Encoding.ASCII))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            // do something with line

                            // Receive the response from the remote device.  
                            // int bytesRec = sender.Receive(bytes);
                            Console.WriteLine("Echoed test = {0}",
                                line);
                            recived = line+"\r\n";
                            Console.WriteLine("Recived: " + recived);
                            // Release the socket.  
                            if (recived.Contains("LOGOUT") && recived.Contains("DONE"))
                                UserName = null;
                            else if (recived.Contains("REG"))
                                businessLayer.getBL().registered(recived);
                            else if (recived.Contains("LOGIN"))
                                businessLayer.getBL().loggedin(recived);
                            else if (recived.Contains("EDITPASS"))
                                businessLayer.getBL().edittedUserPassword(recived);
                            else if (recived.Contains("EDITUSERNAME"))
                                businessLayer.getBL().edittedUserName(recived);
                            else if (recived.Contains("EDITUSEREMAIL"))
                                businessLayer.getBL().edittedUserEmail(recived);
                            else if (recived.Contains("SEARCHGAMESBYPOTSIZE"))
                                businessLayer.getBL().searchedGamesByPotSize(recived);
                            else if (recived.Contains("SEARCHGAMESBYPREFS"))
                                businessLayer.getBL().searchedGamesByPotSize(recived);
                            else if (recived.Contains("SEARCHGAMESBYPLAYERNAME"))
                                businessLayer.getBL().searchedGamesByPotSize(recived);
                            else if (recived.Contains("LISTSPECTATEABLEGAMES"))
                                businessLayer.getBL().searchedGamesByPotSize(recived);
                            else if (recived.Contains("LISTJOINABLEGAMES"))
                                businessLayer.getBL().searchedGamesByPotSize(recived);
                            else if (recived.Contains("JOINGAME"))
                                businessLayer.getBL().joinedGame(recived);
                            else if (recived.Contains("SPECTATEGAME"))
                                businessLayer.getBL().spectated(recived);
                            else if (recived.Contains("CREATEGAME"))
                                businessLayer.getBL().createdGame(recived);
                            else if (recived.Contains("EDITUSERAVATAR"))
                                businessLayer.getBL().edittedUserAvatar(recived);
                            else if (recived.Contains("TAKEACTION"))
                                businessLayer.getBL().takeAction(recived);
                            else if (recived.Contains("ACTION"))
                                businessLayer.getBL().ActionMakedd(recived);
                            else if (recived.Contains("CHATMSG"))
                                businessLayer.getBL().reciveMsgToChat(recived);
                            else if (recived.Contains("WHISPERMSG"))
                                businessLayer.getBL().reciveWhisper(recived);
                            else if (recived.Contains("LEAVEGAME"))
                                businessLayer.getBL().leavedGame(recived);
                            else if (recived.Contains("GAMEUPDATE"))
                                businessLayer.getBL().gameUpdated(recived);
                            else if (recived.Contains("GAMEREPLAY"))
                                businessLayer.getBL().gameReplayed(recived);
                            else Console.WriteLine("ERROR BAD INSTRUCTION:" + recived);



                        }
                    }




                }
                client.Close();

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }

            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
            finally { client.Close(); }

        }

        public bool send(string msg)
        {
            byte[] bytes = new byte[4096];
            StreamWriter writer = new StreamWriter(stream, Encoding.ASCII);
            writer.AutoFlush = true;
            try
            { // Encode the data string into a byte array.  
             //   bytes = Encoding.ASCII.GetBytes(msg);
                Console.WriteLine("Sending: " + msg);
                // Send the data through the socket.  
                //  int bytesSent = sender.Send(bytes);
                writer.WriteLine(msg.Substring(0,msg.Length-1));
                //Console.WriteLine("number of bytes send: " + bytesSent);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                return false;
            }

        }

        public static int START()
        {
            communicationLayer CL = new communicationLayer();


            return 0;
        }


    }
}

