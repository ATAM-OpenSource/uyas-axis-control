using Microsoft.Web.WebView2.Core;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace UYASAxisControl
{

	public partial class Form1 : Form
	{

		public Form1()
		{
			InitializeComponent();
			this.Resize += new System.EventHandler(this.Form_Resize);

			InitializeAsync();

			this.WindowState = FormWindowState.Normal;
			//this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			//this.Bounds = Screen.PrimaryScreen.Bounds;

			//this.WindowState = FormWindowState.Maximized;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
		}

		private void Form_Resize(object sender, EventArgs e)
		{
			webView.Size = this.ClientSize - new System.Drawing.Size(webView.Location);
		}

		async void InitializeAsync()
		{
			await webView.EnsureCoreWebView2Async(null);

			//string html = System.IO.File.ReadAllText("html/index.html");
			//this.webView.CoreWebView2.NavigateToString(html);
			this.webView.CoreWebView2.Navigate("https://signalregistry.net");

			this.webView.CoreWebView2.OpenDevToolsWindow();

			this.webView.NavigationCompleted += WebView_NavigationCompleted;
			this.webView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;


			// await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("alert(window.document.URL);");
			// await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
			// await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.addEventListener(\'message\', event => alert(event.data));");
			Debug.WriteLine("[DEBUG] Webivew form loaded.");
		}

		private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
		{
			//throw new NotImplementedException();
			Debug.WriteLine("[DEBUG] Message received from page: " + e.TryGetWebMessageAsString());
			//Debug.WriteLine(e.TryGetWebMessageAsString());
		}

		private void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
		{
			this.webView.CoreWebView2.Settings.IsScriptEnabled = true;
			this.webView.CoreWebView2.ExecuteScriptAsync("window.chrome.webview.addEventListener('message', (event) => {console.log(event.data)})");
			this.webView.CoreWebView2.PostWebMessageAsString("[DEBUG] Message from main program.");
			Debug.WriteLine("[DEBUG] Navigation Completed.");
		}
	}
}

