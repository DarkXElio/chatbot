using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatbotServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Lister: in ascolto quando si parla dei server
            // EndPoint: identifica una coppia IP/Porta

            //Creare il mio socketlistener
            //1) specifico che versione IP
            //2) tipo di socket. Stream.
            //3) protocollo a livello di trasporto
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                                    ProtocolType.Tcp);
            // config: IP dove ascoltare. Possiamo usare l'opzione Any: ascolta da tutte le interfaccie all'interno del mio pc.
            IPAddress ipaddr = IPAddress.Any;

            // config: devo configurare l'EndPoint
            IPEndPoint ipep = new IPEndPoint(ipaddr, 23000);

            // config: Bind -> collegamento
            // listenerSocket lo collego all'endpoint che ho appena configurato
            listenerSocket.Bind(ipep);

            // Mettere in ascolto il server.
            // parametro: il numero massimo di connessioni da mettere in coda.
            listenerSocket.Listen(5);
            Console.WriteLine("Server in ascolto...");
            Console.WriteLine("in attesa di connessione da parte del client...");
            // Istruzione bloccante
            // restituisce una variabile di tipo socket.
            Socket client = listenerSocket.Accept();

            Console.WriteLine("Client IP: " + client.RemoteEndPoint.ToString());

            // mi attrezzo per ricevere un messaggio dal client
            // siccome è di tipo stream io riceverò dei byte, o meglio un byte array
            // riceverò anche il numero di byte.
            byte[] buff = new byte[128];
            int receivedBytes = 0;
            int sendedBytes = 0;
            string receivedString, sendString;
            string bot = "ChatBot :  Benvenuto client\n\r\n\r" +
                         "ChatBot : Se ti server aiuta basta inviare \n\r" +
                         "#CMD  ( Per vederi tutti i commandi )\n\r\n \r" +
                         "Client:";

            bool flags = false;





            //invio al client il messaggio del benvenuto
            buff = Encoding.ASCII.GetBytes(bot);
            sendedBytes = client.Send(buff);



            //Multi messaggio
            Random rnd = new Random();
            while (true)
            {
                try
                {
                receivedBytes = client.Receive(buff);
                
                Console.WriteLine("Numero di byte ricevuti: " + receivedBytes);
                receivedString = Encoding.ASCII.GetString(buff, 0, receivedBytes);
                Console.WriteLine("Stringa ricevuta: " + receivedString);

                if(receivedString != "\r\n")
                {

                
                if (receivedString.ToUpper() == "CIAO")
                {
                        String[] Msg_risp_ciao = { "Ciao", "Buongiorno", "Salve", "Ciao come posso aiutarti?" };

                          bot = "ChatBot : "+ Msg_risp_ciao[rnd. Next(5)]+" \n \r" +
                                "Client:";

                    flags = true;
                }

                if (receivedString.ToUpper() == "COME STAI?")
                {
                    String[] Msg_ris_come_stai= { "Bene", "Male", "Cosi cosi", "Eh il nuovo DPCM ci ha chiuso in casa" };
                   
                          bot = "ChatBot : " + Msg_ris_come_stai[rnd.Next(5)] + " \n \r" +
                                "Client:";

                    flags = true;
                }
                if (receivedString.ToUpper() == "CHE FAI?")
                {
                    String[] Msg_risp_che_fai = { "Niente", "Ti sto rsipondento", "Lavoro", "Guardando video su TikTok" };
                          bot = "ChatBot: " + Msg_risp_che_fai[rnd.Next(5)] + " \n \r" +
                                "Client:";
                    flags = true;
                }
                if (receivedString.ToUpper() == "CMD")
                {
                          bot = "ChatBot: Posso rispondere solo alle seguenti domande \n\r" +
                                "#Ciao \n\r" +
                                "#Come stai?\n\r" +
                                "#Che fai?\n\r" +
                                "ChatBot: Se vuoi uscire dal Bot basta inviare \n\r" +
                                "#Quit\n\r" +
                                "#Exit \n \r" +
                                "Client:";
                    flags = true;
                }

                if (receivedString.ToUpper() != "CHE FAI?" && receivedString.ToUpper() != "CIAO" && receivedString.ToUpper() != "COME STAI?" && flags == false)
                {
                            String[] Msg_risp_nonhocapito = { "Non credo di aver trovato la risposta alla tua domanda", "Non ho capito", "Posso rispondere solo alle seguenti domande \n\r-Ciao \n\r-Come stai?\n\r-Che fai?", "Mi dispiace ma non ho capito" };
                          
                          bot = "ChatBot : " + Msg_risp_nonhocapito[rnd.Next(5)] + " \n \r" +
                                "Client:";
                    
                }

                if (receivedString.ToUpper() == "QUIT" || receivedString.ToUpper() == "EXIT")
                        {
                    bot = "ChatBot : Arrivderci :)";
                    break;
                }

                flags = false;
                Array.Clear(buff, 0, buff.Length);
                sendedBytes = 0;

                // crea il messaggio
                sendString = bot;

                // lo converto in byte
                buff = Encoding.ASCII.GetBytes(sendString);

                //invio al client il messaggio
                sendedBytes = client.Send(buff);
                
                Array.Clear(buff, 0, buff.Length);
                }
                }
                catch
                {
                    Console.WriteLine("Qualcosa è andato storto Errore di disconessione");
                    Console.WriteLine("Premi Enter per uscire");
                    Console.ReadLine();
                    break;
                }

            }
            
            // Termina il programma


        }
    }
}
