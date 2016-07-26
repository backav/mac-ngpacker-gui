using System;

using Foundation;
using AppKit;
using System.Diagnostics;
using System.IO;

namespace MakeChannel
{
	public partial class MainWindowController : NSWindowController
	{
		public MainWindowController (IntPtr handle) : base (handle)
		{
		}

		[Export ("initWithCoder:")]
		public MainWindowController (NSCoder coder) : base (coder)
		{
		}

		public MainWindowController () : base ("MainWindow")
		{
			
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();

			NSUserDefaults nud = NSUserDefaults.StandardUserDefaults;

			txtApk.StringValue= nud.StringForKey("apk") ?? "";
			txtKeystore.StringValue = nud.StringForKey("keystore") ?? "";
			txtKeyStorePasswd.StringValue = nud.StringForKey ("keystore_passwd") ?? "";
			txtAlias.StringValue = nud.StringForKey ("alias") ?? "";
			txtPassword.StringValue = nud.StringForKey ("password") ?? "";
			txtChannels.TextStorage.SetString( new NSAttributedString(nud.StringForKey ("channels") ?? ""));
			txtSaveLocation.StringValue = nud.StringForKey ("save_location") ?? "";
		}

		public new MainWindow Window {
			get { return (MainWindow)base.Window; }
		}

		partial void selectApk (NSObject sender)
		{
			String file=selectFile();
			if (file != null){
				txtApk.StringValue=file;
			}
		}
		partial void selectKeystore (NSObject sender)
		{
			String file=selectFile();
			if (file != null){
				txtKeystore.StringValue=file;
			}
		}
		partial void selectSaveLocation (NSObject sender)
		{
			String file=selectDictionary();
			if(file != null){
				txtSaveLocation.StringValue=file;
			}
		}



		System.Text.StringBuilder outputs;
		partial void generage (NSObject sender)
		{
			saveAsDefault();

			outputs=new System.Text.StringBuilder();
			String command = "jarsigner -digestalg SHA1 -sigalg SHA1withRSA -keystore \""+txtKeystore.StringValue +"\" -storepass "+txtKeyStorePasswd.StringValue+" -keypass "+txtPassword.StringValue+" \""+txtApk.StringValue +"\" " + txtAlias.StringValue;

			String channelConfigFilePath=NSBundle.MainBundle.PathForResource("channels","txt");

			NSFileHandle fh= NSFileHandle.OpenWrite(channelConfigFilePath);
			fh.TruncateFileAtOffset(0);
			fh.WriteData(NSData.FromString(txtChannels.Value));
			fh.CloseFile();


			// sign
			if(runCommand(command)){

				String output=txtApk.StringValue.Replace(".apk","-NoChannel.apk");
				String zipalign=NSBundle.MainBundle.PathForResource("zipalign",null);
				command = zipalign +" -f 4  \"" + txtApk.StringValue +"\" \""+output+"\"";
				if(runCommand(command)){

					String ngparker=NSBundle.MainBundle.PathForResource("ngpacker",".py");
					command = "/usr/bin/env python \""+ngparker +"\" \""+output+"\" \""+channelConfigFilePath + "\" \""+txtSaveLocation.StringValue+"\"";
					if(runCommand(command)){
						alert("打包成功");
					}else{
						alert("打包失败");
					}

					NSError error;
					NSFileManager fm= NSFileManager.DefaultManager;
					fm.Remove(output,out error);
				
				}else{
					alert("Align fail");
				}

				;
			}else{
				alert("签名失败");
			}
		}

		private bool runCommand(String command){
			Process proc = new System.Diagnostics.Process();

			proc.StartInfo.FileName = "/bin/bash";
			proc.StartInfo.Arguments = "-c '"+command+"'";
			proc.StartInfo.UseShellExecute = false; 
			proc.StartInfo.RedirectStandardOutput = true;
			proc.StartInfo.RedirectStandardError = true;
			proc.ErrorDataReceived+=new DataReceivedEventHandler(NetOutputDataHandler);
			proc.OutputDataReceived+=new DataReceivedEventHandler(NetOutputDataHandler);
			proc.Start();
			proc.BeginErrorReadLine();
			proc.BeginOutputReadLine();

			outputs.Append(":");
			outputs.Append(command);
			outputs.AppendLine();
			txtOutputs.TextColor = NSColor.White;
			proc.WaitForExit ();
			return proc.ExitCode == 0;
		}



		private void NetOutputDataHandler(object sendingProcess, 
			DataReceivedEventArgs outLine)
		{
			// Collect the net view command output.
			if (!String.IsNullOrEmpty(outLine.Data))
			{
				outputs.Append (outLine.Data);
				outputs.AppendLine ();
				txtOutputs.BeginInvokeOnMainThread (() => {

					txtOutputs.Value = outputs.ToString ();
					txtOutputs.ScrollRangeToVisible(new NSRange(txtOutputs.Value.Length, 0));
				});
			}
		}

		private void saveAsDefault(){
			NSUserDefaults nud = NSUserDefaults.StandardUserDefaults;
			nud.SetString (txtApk.StringValue, "apk");
			nud.SetString (txtApk.StringValue,"apk");
			nud.SetString (txtKeystore.StringValue,"keystore");
			nud.SetString (txtKeyStorePasswd.StringValue ,"keystore_passwd");
			nud.SetString (txtAlias.StringValue, "alias");
			nud.SetString (txtPassword.StringValue ,"password");
			nud.SetString (txtChannels.Value ,"channels");
			nud.SetString (txtSaveLocation.StringValue, "save_location");
		}

		private void alert(String txt){
			var alertBox = new NSAlert {
				MessageText = txt,
				AlertStyle = NSAlertStyle.Informational
			};

			alertBox.AddButton ("OK");

			alertBox.RunModal();
		}

		private String selectFile(){
			var panel = NSOpenPanel.OpenPanel;
			panel.FloatingPanel = true;
			panel.CanChooseDirectories = false;
			panel.CanChooseFiles = true;

			nint i = panel.RunModal ();
			if (i == 1 && panel.Urls != null) {
				return Uri.UnescapeDataString(panel.Urls[0].Path);
			}
			return null;
		}
		private String selectDictionary(){
			var panel = NSOpenPanel.OpenPanel;
			panel.FloatingPanel = true;
			panel.CanChooseDirectories = true;
			panel.CanChooseFiles = false;

			nint i = panel.RunModal ();
			if (i == 1 && panel.Urls != null) {
				return Uri.UnescapeDataString(panel.Urls[0].Path);
			}
			return null;
		}

	}
}
