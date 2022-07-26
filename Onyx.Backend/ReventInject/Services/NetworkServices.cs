using System;
using System.Net;

namespace ReventInject.Services
{

	public class ServerInfo
	{
		public string HostName { get; set; }
		public string IPAddress { get; set; }
	}

	public class NetworkService
	{
		private string _HostName;
		private string _IPAddress;
		public ServerInfo serverInfo { get; set; }

		public NetworkService()
		{
			LoadServerInfo();
		}

		public NetworkService(ref ServerInfo servInfo)
		{
			LoadServerInfo();
			servInfo.HostName = _HostName;
			servInfo.IPAddress = _IPAddress;
		}

		private void LoadServerInfo()
		{
			try
			{
				_HostName = System.Net.Dns.GetHostName();
				_IPAddress = System.Net.Dns.GetHostEntry(_HostName).AddressList[1].ToString();

				serverInfo = new ServerInfo
				{
					HostName = _HostName,
					IPAddress = _IPAddress
				};


			}
			catch (Exception ex)
			{
			}
		}

		public ServerInfo GetServerInfo()
		{

			dynamic serv = new ServerInfo();
			try
			{
				serv.HostName = _HostName;
				serv.IPAddress = _IPAddress;

			}
			catch (Exception ex)
			{
			}

			return serv;

		}

		public string GetIPAddress()
		{

			try
			{
				return _IPAddress;

			}
			catch (Exception ex)
			{
				return null;
			}

		}

		public string GetHostName()
		{

			try
			{
				return _HostName;

			}
			catch (Exception ex)
			{
				return null;
			}

		}

		public string GetIPv4Address()
		{
			dynamic IPv4Address = string.Empty;
			string strHostName = System.Net.Dns.GetHostName();
			System.Net.IPHostEntry iphe = System.Net.Dns.GetHostEntry(strHostName);

			foreach (System.Net.IPAddress ipheal in iphe.AddressList)
			{
				if (ipheal.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
				{
					IPv4Address = ipheal.ToString();
				}
			}
			return IPv4Address;
		}

		public string getClientIPAddress()
		{

			string clientName = Environment.MachineName;
			try
			{
				//Dim IPHost As Net.IPHostEntry = Net.Dns.GetHostEntry(clientName & ".gtalimited.com")
				System.Net.IPHostEntry IPHost = System.Net.Dns.GetHostEntry("");
				if (IPHost.AddressList.Length > 0)
				{
					System.Net.IPAddress[] addresses = IPHost.AddressList;
					dynamic IP = addresses[1].ToString();
					//Console.WriteLine(IP)
					return IP;
				}

			}
			catch (Exception ex)
			{
			}

			return null;

		}

		public IPAddress[] getClientIPAddressList()
		{

			System.Net.IPHostEntry ipentry = System.Net.Dns.GetHostEntry("");
			try
			{
				if (ipentry != null & ipentry.AddressList.Length > 0)
				{
					return ipentry.AddressList;
				}

			}
			catch (Exception ex)
			{
			}

			return null;

		}

		public IPHostEntry getClientIPHost()
		{

			string clientName = Environment.MachineName;
			try
			{
				//Dim IPHost As Net.IPHostEntry = Net.Dns.GetHostEntry(clientName & ".gtalimited.com")
				System.Net.IPHostEntry IPHost = System.Net.Dns.GetHostEntry("");
				if (IPHost != null & IPHost.AddressList.Length > 0)
				{
					return IPHost;
				}

			}
			catch (Exception ex)
			{
			}

			return null;

		}

		public string GetUser_IP()
		{
			string VisitorsIPAddr = string.Empty;
			//var remote_ip = _httpContextAccessor.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			//         if (remote_ip != null)
			//         {
			//             VisitorsIPAddr = remote_ip.ToString();
			//         }
			//         else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
			//         {
			//             VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
			//         }

			return VisitorsIPAddr;
		}

		/// <summary>
		/// method to get Client ip address
		/// </summary>
		/// <param name="GetLan"> set to true if want to get local(LAN) Connected ip address</param>
		/// <returns></returns>
		public string GetVisitorIPAddress(bool GetLan = false)
		{
			string ipAddr = "";
			//_httpContextAccessor.HttpContext.Request.Headers["HTTP_X_FORWARDED_FOR"];

			//if (String.IsNullOrEmpty(ipAddr))
			//    ipAddr = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

			//if (string.IsNullOrEmpty(ipAddr))
			//    ipAddr = HttpContext.Current.Request.UserHostAddress;

			//if (string.IsNullOrEmpty(ipAddr) || ipAddr.Trim() == "::1")
			//{
			//    GetLan = true;
			//    ipAddr = string.Empty;
			//}

			//if (GetLan && string.IsNullOrEmpty(ipAddr))
			//{
			//    //This is for Local(LAN) Connected ID Address
			//    string stringHostName = Dns.GetHostName();
			//    //Get Ip Host Entry
			//    IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
			//    //Get Ip Address From The Ip Host Entry Address List
			//    IPAddress[] arrIpAddress = ipHostEntries.AddressList;

			//    try
			//    {
			//        ipAddr = arrIpAddress[arrIpAddress.Length - 2].ToString();
			//    }
			//    catch
			//    {
			//        try
			//        {
			//            ipAddr = arrIpAddress[0].ToString();
			//        }
			//        catch
			//        {
			//            try
			//            {
			//                arrIpAddress = Dns.GetHostAddresses(stringHostName);
			//                ipAddr = arrIpAddress[0].ToString();
			//            }
			//            catch
			//            {
			//                ipAddr = "127.0.0.1";
			//            }
			//        }
			//    }

			//}


			return ipAddr;
		}

	}

}
