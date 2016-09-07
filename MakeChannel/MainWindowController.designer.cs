// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MakeChannel
{
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		AppKit.NSTextField txtAlias { get; set; }

		[Outlet]
		AppKit.NSTextField txtApk { get; set; }

		[Outlet]
		AppKit.NSTextView txtChannels { get; set; }

		[Outlet]
		AppKit.NSTextField txtKeystore { get; set; }

		[Outlet]
		AppKit.NSTextField txtKeyStorePasswd { get; set; }

		[Outlet]
		AppKit.NSTextView txtOutputs { get; set; }

		[Outlet]
		AppKit.NSTextField txtPassword { get; set; }

		[Outlet]
		AppKit.NSTextField txtSaveLocation { get; set; }

		[Action ("generage:")]
		partial void generage (Foundation.NSObject sender);

		[Action ("justSign:")]
		partial void justSign (Foundation.NSObject sender);

		[Action ("selectApk:")]
		partial void selectApk (Foundation.NSObject sender);

		[Action ("selectKeystore:")]
		partial void selectKeystore (Foundation.NSObject sender);

		[Action ("selectSaveLocation:")]
		partial void selectSaveLocation (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (txtAlias != null) {
				txtAlias.Dispose ();
				txtAlias = null;
			}

			if (txtApk != null) {
				txtApk.Dispose ();
				txtApk = null;
			}

			if (txtChannels != null) {
				txtChannels.Dispose ();
				txtChannels = null;
			}

			if (txtKeystore != null) {
				txtKeystore.Dispose ();
				txtKeystore = null;
			}

			if (txtKeyStorePasswd != null) {
				txtKeyStorePasswd.Dispose ();
				txtKeyStorePasswd = null;
			}

			if (txtOutputs != null) {
				txtOutputs.Dispose ();
				txtOutputs = null;
			}

			if (txtPassword != null) {
				txtPassword.Dispose ();
				txtPassword = null;
			}

			if (txtSaveLocation != null) {
				txtSaveLocation.Dispose ();
				txtSaveLocation = null;
			}
		}
	}
}
