using System;
using UIKit;

namespace XamAppCenterSample2018.iOS
{
    public class LinkerPleaseInclude
    {
        public void Include(UITextView textView)
        {
            textView.Text = textView.Text + "";
            textView.Changed += (s, e) => { textView.Text = ""; };
            textView.TextStorage.DidProcessEditing += (s, e) => textView.Text = "";
        }

        public void Include(UIButton uiButton)
        {
            uiButton.TouchUpInside += (s, e) =>
                                      uiButton.SetTitle(uiButton.Title(UIControlState.Normal), UIControlState.Normal);
        }
    }
}
