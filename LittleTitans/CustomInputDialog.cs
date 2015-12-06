using System;

namespace LittleTitans
{
	public partial class CustomInputDialog : Gtk.Dialog
	{
		public CustomInputDialog ()
		{
			this.Build ();
		}

		protected void OnButtonOkClicked (object sender, EventArgs e)
		{
			this.Respond (Gtk.ResponseType.Ok);
		}

		protected void OnButtonCancelClicked (object sender, EventArgs e)
		{
			this.Respond (Gtk.ResponseType.Cancel);
		}

		public String Text {
			get {
				return textview1.Buffer.Text;
			}
		}
	}
}

