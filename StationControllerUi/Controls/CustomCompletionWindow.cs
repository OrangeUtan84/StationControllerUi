using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationControllerUi.Controls
{
    public class CustomCompletionWindow : CompletionWindow
    {
        public CustomCompletionWindow(TextArea textArea) : base(textArea)
        {
            CompletionList.CompletionData.Add(new CustomCompletionData("if", "if statement"));
            CompletionList.CompletionData.Add(new CustomCompletionData("else", "else block of an if statement"));
            CompletionList.CompletionData.Add(new CustomCompletionData("else_if", "else if block of an if statment"));
            CompletionList.CompletionData.Add(new CustomCompletionData("let", "declare a new local variable"));
            CompletionList.CompletionData.Add(new CustomCompletionData("glet", "declare a global variable"));
            CompletionList.CompletionData.Add(new CustomCompletionData("print", "print something to the screen"));
            CompletionList.CompletionData.Add(new CustomCompletionData("send", "send sth. to a socket connection"));
            CompletionList.CompletionData.Add(new CustomCompletionData("label", "defines a new subroutine"));
            CompletionList.CompletionData.Add(new CustomCompletionData("goto", "jumps to a subroutine"));
            CompletionList.CompletionData.Add(new CustomCompletionData("gosub", "jumps to a subroutine and continues at current position after subroutine finished"));
            CompletionList.CompletionData.Add(new CustomCompletionData("return", "returns from a subroutine"));
            CompletionList.CompletionData.Add(new CustomCompletionData("define", "defines a constant"));
            CompletionList.CompletionData.Add(new CustomCompletionData("open", "opens a socket connection"));
        }
    }
}
