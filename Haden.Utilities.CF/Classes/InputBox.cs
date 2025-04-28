using System;

namespace Haden.Utilities.CF
{
	/// <summary>
	/// Summary description for TetraInputBox.
	/// </summary>
	public class InputBox {
        private InputBox()
        {
            //
			// TODO: Add constructor logic here
			//
		}

		public static bool Show(string question, out string answer) {
			return Show(question, out answer, "");
		}

		public static bool Show(string question, out string answer, string current) {
            InputBoxForm form = new InputBoxForm();
            form.Answer = current;
			form.Text = question;
			form.ShowDialog();
			answer = form.Answer;
			return form.Valid;
		}
	}

}
