using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.XPath;
using UnityEngine;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class BingAPIManager {
	static string accountKey = "OMgW+eA9+DXa8XgthKXOPB5D6yqHAvWN6fDwwy7W9FU";
	static string rootURI = "https://api.datamarket.azure.com/Bing/Search/v1/Composite";

	public static void executeSearch(string query) {
		string sources = "web";

		// Create the request URL
		string request = rootURI + "?"; // Root URI with parameters
		request += "Query=" + "\'" + query + "\'"; // Insert the query
		request += "&";
		request += "Sources=" + "\'" + sources + "\'"; // Insert the sources
		request += "&";
		request += "$format=" + "JSON"; // Response in JSON

		// Make the request
		HttpWebRequest webRequest = WebRequest.Create(request) as HttpWebRequest;
		FixCertificateProblems();
		String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(accountKey + ":" + accountKey));
		webRequest.Headers.Add("Authorization", "Basic " + encoded);

		HttpWebResponse response = webRequest.GetResponse() as HttpWebResponse;

		JObject responseObj;

		using (var sr = new StreamReader(response.GetResponseStream()))
		//using (var sr = File.OpenText(@"D:\Programming\Unity\CodeTesting\Assets\ObjectMoving\Scripts\response.txt"))
		using (var jsonTextReader = new JsonTextReader(sr)) {
			responseObj = (JObject) JToken.ReadFrom(jsonTextReader);
		}

        SearchResultsDisplayer.ParseResults(responseObj);
	}

	

	private static void FixCertificateProblems() {
		ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
	}

	public static bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
		bool isOk = true;
		// If there are errors in the certificate chain, look at each error to determine the cause.
		if (sslPolicyErrors != SslPolicyErrors.None) {
			for (int i=0; i<chain.ChainStatus.Length; i++) {
				if (chain.ChainStatus [i].Status != X509ChainStatusFlags.RevocationStatusUnknown) {
					chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
					chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
					chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan (0, 1, 0);
					chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
					bool chainIsValid = chain.Build ((X509Certificate2)certificate);
					if (!chainIsValid) {
						isOk = false;
					}
				}
			}
		}
		return isOk;
	}
}