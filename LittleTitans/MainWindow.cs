using System;
using Gtk;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using LittleTitans;


public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		console.ModifyBase (StateType.Normal, new Gdk.Color (0000, 0000, 0000));
		console.ModifyText (StateType.Normal, new Gdk.Color (0000, 0255, 0000));
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnFileOpSelectionChanged (object sender, EventArgs e){
		System.IO.StreamReader file = System.IO.File.OpenText (fileOp.Filename);
		Regex filetype = new Regex ("[.lol]$");
		Match match = filetype.Match (fileOp.Filename);
		if (match.Success) {
			console.Buffer.Text += ">"+ fileOp.Filename + " successfully loaded!\n";
			coded.Buffer.Text = file.ReadToEnd ();
		}
		else{
			console.Buffer.Text += ">" + fileOp.Filename + " not loaded. It is not a .lol file.\n";
		file.Close ();
		}
	}

	protected void OnExeClicked (object sender, EventArgs e)
	{
		Gtk.TreeViewColumn lexColumn = new Gtk.TreeViewColumn ();
		lexColumn.Title = "Lexemes";
		Gtk.CellRendererText lexCell = new Gtk.CellRendererText ();
		lexColumn.PackStart (lexCell, true);

		Gtk.TreeViewColumn classColumn = new Gtk.TreeViewColumn ();
		classColumn.Title = "Classification";
		Gtk.CellRendererText classCell = new Gtk.CellRendererText ();
		classColumn.PackStart (classCell, true);

		//for deleting previous columns
		foreach (TreeViewColumn col in lexemes.Columns) {
			lexemes.RemoveColumn (col);
		}

		// Add the columns to the TreeView
		lexemes.AppendColumn (lexColumn);
		lexemes.AppendColumn (classColumn);

		// Tell the Cell Renderers which items in the model to display
		lexColumn.AddAttribute (lexCell, "text", 0);
		classColumn.AddAttribute (classCell, "text", 1);

		// Create a model that will hold two strings - lexemes and classification
		Gtk.ListStore lex = new Gtk.ListStore (typeof (string), typeof (string));
		// Assign the model to the TreeView
		lexemes.Model = lex;


		Gtk.TreeViewColumn modColumn = new Gtk.TreeViewColumn ();
		modColumn.Title = "Modifier";
		Gtk.CellRendererText modCell = new Gtk.CellRendererText ();
		modColumn.PackStart (modCell, true);

		Gtk.TreeViewColumn valColumn = new Gtk.TreeViewColumn ();
		valColumn.Title = "Value";
		Gtk.CellRendererText valCell = new Gtk.CellRendererText ();
		valColumn.PackStart (valCell, true);

		//for deleting previous columns
		foreach (TreeViewColumn col in symbol.Columns) {
			symbol.RemoveColumn (col);
		}

		// Add the columns to the TreeView
		symbol.AppendColumn (modColumn);
		symbol.AppendColumn (valColumn);

		// Tell the Cell Renderers which items in the model to display
		modColumn.AddAttribute (modCell, "text", 0);
		valColumn.AddAttribute (valCell, "text", 1);

		// Create a model that will hold two strings - Modifier and Value
		Gtk.ListStore sym = new Gtk.ListStore (typeof (string), typeof (string));

		// Assign the model to the TreeView
		symbol.Model = sym;



		Gtk.TreeViewColumn conColumn = new Gtk.TreeViewColumn ();
		conColumn.Title = "Console";
		Gtk.CellRendererText conCell = new Gtk.CellRendererText ();
		conColumn.PackStart (conCell, true);


		console.Buffer.Text = "";

	


		var regexList = new List<String>();
		var regexClassification = new List<String>();

		regexList.Add ("^-?[0-9]*.[0-9]+");	//numbar literal
		regexClassification.Add ("NUMBAR Literal");

		regexList.Add ("^-?[0-9]+");	//numbr literal
		regexClassification.Add ("NUMBR Literal");

		regexList.Add("^HAI");
		regexClassification.Add ("Start of Program");

		regexList.Add ("^VISIBLE\\s");
		regexClassification.Add ("Output Keyword");

		regexList.Add ("^KTHXBYE");
		regexClassification.Add ("End of Program");

		regexList.Add ("^\".*\"");	//yarn literal
		regexClassification.Add ("YARN Literal");

		regexList.Add ("^(WIN|FAIL)");	//troof literal
		regexClassification.Add ("TROOF Literal");

		regexList.Add ("^(YARN|NUMBAR|TROOF|BUKKIT|NOOB)");
		regexClassification.Add ("TYPE Literal");

		regexList.Add ("^BTW ");
		regexClassification.Add ("Start of Single Line Comment");

		regexList.Add ("^OBTW");
		regexClassification.Add ("Start of Multi Line Comment");

		regexList.Add ("^TLDR\\s*$");
		regexClassification.Add ("End of Multi Line Comment");

		regexList.Add ("^I HAS A ");
		regexClassification.Add ("Variable Declaration");

		regexList.Add ("^ITZ ");
		regexClassification.Add ("Variable Initialization");

		regexList.Add ("^R ");
		regexClassification.Add ("Assignment of Value");

		regexList.Add ("^AN ");
		regexClassification.Add ("And");

		regexList.Add ("^SUM OF ");
		regexClassification.Add ("Addition");

		regexList.Add ("^DIFF OF ");
		regexClassification.Add ("Subtraction");

		regexList.Add ("^PRODUKT OF ");
		regexClassification.Add ("Multiplication");

		regexList.Add ("^QUOSHUNT OF ");
		regexClassification.Add ("Division");

		regexList.Add ("^MOD OF ");
		regexClassification.Add ("Modulo");

		regexList.Add ("^BIGGR OF ");
		regexClassification.Add ("Larger of Two Numbers");

		regexList.Add ("^SMALLR OF ");
		regexClassification.Add ("Smaller of Two Numbers");

		regexList.Add ("^BOTH OF ");
		regexClassification.Add ("Logical And");

		regexList.Add ("^EITHER OF ");
		regexClassification.Add ("Logical Or");

		regexList.Add ("^WON OF ");
		regexClassification.Add ("Logical Xor");

		regexList.Add ("^NOT ");
		regexClassification.Add ("Logical Not");

		regexList.Add ("^ALL OF ");
		regexClassification.Add ("Logical And For Number of Arguments");

		regexList.Add ("^ANY OF ");
		regexClassification.Add ("Logical Or For Number of Arguments");

		regexList.Add ("^BOTH SAEM ");
		regexClassification.Add ("Comparing If Equal");

		regexList.Add ("^DIFFRINT ");
		regexClassification.Add ("Comparing If Different");

		regexList.Add ("^SMOOSH ");
		regexClassification.Add ("Yarn Concatenation");

		regexList.Add ("^MAEK ");
		regexClassification.Add ("Expression Type Casting");

		regexList.Add ("^IS NOW A ");
		regexClassification.Add ("Variable Type Casting");

		regexList.Add ("^GIMMEH ");
		regexClassification.Add ("Scan Keyword");

		regexList.Add ("^O RLY\\?");
		regexClassification.Add ("If-Else Start");

		regexList.Add ("^YA RLY");
		regexClassification.Add ("If Part");

		regexList.Add ("^NO WAI");
		regexClassification.Add ("Else Part");

		regexList.Add ("^OIC");
		regexClassification.Add ("Control Statement End");

		regexList.Add ("^WTF\\?");
		regexClassification.Add ("Switch Case Start");

		regexList.Add ("^OMGWTF");
		regexClassification.Add ("Default Switch");

		regexList.Add ("^GTFO");
		regexClassification.Add ("Break");

		regexList.Add ("^OMG");
		regexClassification.Add ("Switch");

		regexList.Add ("^IM IN YR");
		regexClassification.Add ("Loop Start");

		regexList.Add ("^UPPIN");
		regexClassification.Add ("Increment Counter");

		regexList.Add ("^NERFIN");
		regexClassification.Add ("Derecement Counter");

		regexList.Add ("^YR");
		regexClassification.Add ("Statement For Loop");

		regexList.Add ("^(TIL|WILE)");
		regexClassification.Add ("Loop Condition");

		regexList.Add ("^IM OUTTA YR");
		regexClassification.Add ("Loop End");

		regexList.Add ("^\"");
		regexClassification.Add ("String Delimiter");

		regexList.Add ("^[A-Za-z][A-Za-z0-9_]*");
		regexClassification.Add ("Variable Identifier");

		Regex keywords = new Regex ("^HAI$|^KTHXBYE$|^VISIBLE$|^WIN$|^FAIL$|^YARN$|^NUMBR$|^NUMBAR$|^TROOF$|^BUKKIT$|^NOOB$|^BTW$|^OBTW$|^TLDR$|^I$|^HAS$|^A$|^ITZ$|^R$|^AN$|^OF$|^SUM$|^DIFF$|^PRODUKT$|^QUOSHUNT$|^MOD$|^BIGGR$|^SMALLR$|^BOTH$|^EITHER$|^WON$|^NOT$|^ALL$|^ANY$|^BOTH$|^SAEM$|^DIFFRINT$|^SMOOSH$|^MAEK$|^IS$|^NOW$|^GIMMEH$|^O$|^RLY$|^NO$|^WAI$|^OIC$|^OMGWTF$|^OMG$|^IM$|^IN$|^YR$|^OUTTA$|^NERFIN$|^UPPIN$|^TIL$|^WILE$");
		//Regex keywords = new Regex ("TEST|HAI");

		Regex arithmeticOperations = new Regex ("^SUM OF|^DIFF OF|^PRODUKT OF|^QUOSHUNT OF|^MOD OF|^BIGGR OF|^SMALLR OF");
		Regex variable = new Regex ("^[A-Za-z][A-Za-z0-9_]*$");

		//console.Buffer.Text = "";
		String codes = coded.Buffer.Text.Trim(); //gets the code from the code text
		String[] lines = codes.Split ('\n'); //splits the code per line
		Dictionary<string, string> mapping = new Dictionary<string,string>();
		mapping.Add ("IT", "NOOB");
		var gimmehValue = "";

		Stack operationStack = new Stack ();
		Stack inverseStack = new Stack ();

		//var regexList = new List<String>();
		//var regexClassification = new List<String>();
		Boolean comparisonChecker = false;
		Boolean evaluateComparison = false;

		Boolean booleanChecker = false;
		Boolean evaluateBoolean = false;
		Boolean allofanyof = false;

		Boolean arithmeticChecker = false;
		Boolean evaluateArithmetic = false;

		Boolean smooshChecker = false;
		Boolean evaluateSmoosh = false;
		String smoosh = "";

		Boolean isFloat = false;
		Boolean btwChecker = false;

		Boolean itz = false;
		Boolean itz_string = false;
		Boolean itz_declaration = false;
		Boolean itz_checkerifelse = false;

		Boolean r = false;
		Boolean r_string = false;
		Boolean r_declaration = false;
		Boolean r_checkerifelse = false;

		Boolean visibleChecker = false;
		Boolean visibleIT = false;

		Boolean obtw = false;
		Boolean stringChecker = false;
		Boolean gimmeh = false;
		Boolean interpreting = true;
		Boolean i_has_a = false;
		Boolean orly = false;
		Boolean wtf = false;
		Boolean wtf_match = false;
		Boolean incorrect_switch = false;
		Boolean gtfo = false;
		Boolean skip_statements = false;

		String long_comment = "";
		var currentVar = "";

		console.Buffer.Text += ">Interpretting code...\n";
		int i = 0;
		if(lines[0].Trim() != "HAI"){
			console.Buffer.Text += ">ERROR AT 1: Invalid start of program. Found '" + lines[0] + "' instead of \"HAI\"'.\n";
			interpreting = false;
		}


		if( lines[lines.Length-1].Trim() != "KTHXBYE"){
			console.Buffer.Text += ">ERROR AT " + lines.Length + ": Invalid end of program. Found '" + lines[lines.Length-1] + "' instead of \"KTHXBYE\".\n";
			interpreting = false;
		}



		else{
			for (i = 0; i < lines.Length; i++) {
				lines [i] = lines [i].Trim ();	//to remove trailing spaces

				//TODO: Evaluate OperationStack after lines[i] is empty

				while (lines [i] != "" && interpreting) {
					for (var j = 0; j < regexList.Count; j++) {
						Regex regex = new Regex (regexList [j]);
						Match match = regex.Match (lines [i]);
						Console.WriteLine (lines [i]);
						if (match.Success) {						
							Console.WriteLine ("match");
							Console.WriteLine (regexList[j]);
							if (regexClassification [j] == "End of Multi Line Comment" && stringChecker == false && visibleChecker == false) {
								if (!obtw) { //ADDED: checks if obtw is set
									console.Buffer.Text += ">ERROR AT " + i + ": no OBTW.\n";
									interpreting = false;
									break;
								}
								lex.AppendValues (long_comment, "Multi Line Comment");
								obtw = false;
								//mapping.Add (match.Value, regexClassification [j]);
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
								break;
							}

							// 'OBTW' will be placed at the first few lines to avoid more error checking
							else if (obtw == true && stringChecker == false && visibleChecker == false) {
								//lex.AppendValues (match.Value, "LONG COMMENT");
								long_comment += lines[i]+='\n';
								lines [i] = lines [i].Remove (0, lines[i].Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
							}

							//FOR 'BTW'
							else if (regexClassification [j] == "Start of Single Line Comment" && !obtw && !stringChecker && !arithmeticChecker && !booleanChecker && !comparisonChecker) {
								//mapping.Add (match.Value, regexClassification [j]);
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
								Console.WriteLine("\thi! im btw\n");
								if (lines [i] == "")
									break;
								btwChecker = false;
								if (visibleChecker) visibleChecker = false; //<-ADDED: removed '!visibleChecker' from conditions and added this statement that unsets visible checker
								lex.AppendValues (lines [i], "Comment String");
								lines [i] = lines [i].Remove (0, lines[i].Length); //<- CHANGED: instead of match.Value.Length which is 3 because match.Value is BTW

								//lines [i] = "";
							}

							else if (!smooshChecker && !arithmeticChecker && !booleanChecker && !comparisonChecker &&  regexClassification [j] == "Variable Identifier" && !obtw && !stringChecker && !visibleChecker && !itz_string && !gimmeh) {
								Console.WriteLine ("Variable Identifier");
								currentVar = match.Value;

								//VARIABLE DECLARATION
								if (i_has_a) {
									//if variable is already declared
									if (mapping.ContainsKey (match.Value)) {
										console.Buffer.Text += ">ERROR AT " + (i + 1) + ": Variable " + match.Value + " already existing. Try changing the name of this variable.\n";
										interpreting = false;
										break;
									} 

									//for checking if it is a reserved word
									else {			
										//CHECK IF reserve word
										Match keywordMatch = keywords.Match (match.Value.Trim());					
										if (!keywordMatch.Success) {
											mapping.Add (match.Value, "NOOB");
											lex.AppendValues (match.Value, regexClassification [j]);
											i_has_a = false;
											var lookAhead = lines [i].Trim ();	
											lookAhead = lookAhead.Remove (0, match.Value.Length);

											if (new Regex ("^OBTW ").Match (lookAhead.Trim()).Success) {
												console.Buffer.Text += ">ERROR AT " + (i) + ": OBTW not allowed\n";
												interpreting = false;
												break;
											}

										} 

									//if not, will allow the declaration of varibale
										else {
												console.Buffer.Text += ">ERROR AT " + (i) + ": Invalid Variable name. '" + match.Value + "' is a reserved word for other functionalities.\n";
												interpreting = false;
												break;
										}
									}
								}

								//FOR DECLARED VARIABLES
								//for checking if there is a variable declared
								else {
									if (!mapping.ContainsKey (match.Value)) {
										console.Buffer.Text += ">ERROR AT " + (i + 1) + ": Variable name. '" + match.Value + "' does not exist.\n";
										interpreting = false;
										break;
									} else {
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();
									}
								}

							}

							//VISIBLE
							else if (regexClassification [j] == "Output Keyword") {
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces

								visibleChecker = true;

							} 

							else if (visibleChecker) {									

								//for visible strings
								if (regexClassification [j] == "YARN Literal") {

									lines [i] = lines [i].Remove (0, match.Value.Length);
									lines [i] = lines [i].Trim ();	//to remove trailing spaces		

									lex.AppendValues ("\"", "String Delimiter");
									string yarn = match.Value.Remove (0, 1).Trim ();
									yarn = yarn.Remove (yarn.Length - 1, 1);
									if(!skip_statements) console.Buffer.Text += yarn += "\n";
									lex.AppendValues (yarn, "YARN Literal");
									lex.AppendValues ("\"", "String Delimiter");

									if (lines [i] == "") {
										visibleChecker = false;
										if(!skip_statements) console.Buffer.Text += "\n";
									}

									stringChecker = false;
								} 

								//for visible variables
								else if (regexClassification [j] == "Variable Identifier" && !stringChecker) {

									lines [i] = lines [i].Remove (0, match.Value.Length);
									lines [i] = lines [i].Trim ();	//to remove trailing spaces		
									//if such variable exists, displays value
									if (mapping.ContainsKey (match.Value)) {
										var value = mapping [match.Value];
										if(!skip_statements) console.Buffer.Text += value.Trim('\"') + "\n";
									} 

									//else will prompt error
									else {
										console.Buffer.Text += ">ERROR AT " + (i + 1) + ": Variable '" + match.Value + "' is not yet declared\n";
										interpreting = false;
										break;

									}
									if (lines [i] == "") {
										visibleChecker = false;
									}
								} 


								//for visible result of operations
								else if (regexClassification [j] == "Comparing If Equal" || regexClassification [j] == "Comparing If Different" || regexClassification [j] == "Logical And" || regexClassification [j] == "Logical Or" || regexClassification [j] == "Logical Xor" || regexClassification [j] == "Logical Not" || regexClassification [j] == "Logical And For Number of Arguments" || regexClassification [j] == "Logical Or For Number of Arguments" || regexClassification [j] == "Smaller of Two Numbers" || regexClassification [j] == "Addition" || regexClassification [j] == "Subtraction" || regexClassification [j] == "Multiplication" || regexClassification [j] == "Division" || regexClassification [j] == "Modulo" || regexClassification [j] == "Larger of Two Numbers" || regexClassification [j] == "Smaller of Two Numbers" || regexClassification[j] == "Yarn Concatenation") {
									Console.WriteLine ("inside visible it");
									visibleIT = true;
									visibleChecker = false;
								}


								//for error checking
								else if (lines [i] == "" && !stringChecker) {
									lines [i] = lines [i].Remove (0, match.Value.Length);
									lines [i] = lines [i].Trim ();	//to remove trailing spaces		

									//missing string delimiter
									if (stringChecker == true) {
										console.Buffer.Text += ">ERROR AT " + (i + 1) + ": Missing string delimiter\n";
										interpreting = false;
										visibleChecker = false;
										break;
									}

									//no more tokens to be checked
									if (lines [i] == "") {
										visibleChecker = false;
									}
								} 

								//check for variable
								else {
									visibleChecker = false;
									lines [i] = lines [i].Remove (0, match.Value.Length);
									lines [i] = lines [i].Trim ();	//to remove trailing spaces		

									if (keywords.Match(match.Value).Success) {
										console.Buffer.Text += ">ERROR AT " + (i + 1) + ": Variable '" + match.Value + "' is not yet declared\n";
										interpreting = false;
										break;
									} 
									lex.AppendValues (match.Value, regexClassification [j]);

									if(!skip_statements) console.Buffer.Text += match.Value + "\n";

								}
								
							}



							//FOR GETTING THE VALUE 'R'
							else if (r && !itz_declaration && !stringChecker && !obtw && !visibleChecker) {

								Console.WriteLine ("current line inside r " + lines [i]);

								//for getting the string
								if (r_string) {

									if (regexClassification [j] == "String Delimiter") {
										r = false;
										r_string = false;
										r_checkerifelse = false;
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces

									} else {
										Console.WriteLine ("inside string == true");
										Console.WriteLine (match.Value);

										var value = mapping [currentVar];
										if (value == "NOOB") {
											if(!skip_statements) mapping [currentVar] = match.Value;
										} else {							
											String stringValue = value + match.Value;
											if(!skip_statements) mapping [currentVar] = stringValue;
										}

										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces
										Console.WriteLine (lines [i]);
										r_checkerifelse = false;

									}
								}


								//if value of variable is stirng
								else if (regexClassification [j] == "String Delimiter") {
									r_string = true;
									r_checkerifelse = false;
									lines [i] = lines [i].Remove (0, match.Value.Length);
									lines [i] = lines [i].Trim ();	//to remove trailing spaces
									//break;
								}

								//if value of variable is determined by an arithmetic operations
								else if (regexClassification [j] == "Addition" || regexClassification [j] == "Subtraction" || regexClassification [j] == "Multiplication" || regexClassification [j] == "Division" || regexClassification [j] == "Modulo" || regexClassification [j] == "Larger of Two Numbers" || regexClassification [j] == "Smaller of Two Numbers") {
									r = false;
									r_declaration = true;
									evaluateArithmetic = true;
								} 


								//if value of variable is determined by boolean operations
								else if (regexClassification [j] == "Logical And" || regexClassification [j] == "Logical Or" || regexClassification [j] == "Logical Xor" || regexClassification [j] == "Logical Not" || regexClassification [j] == "Logical And For Number of Arguments" || regexClassification [j] == "Logical Or For Number of Arguments" || regexClassification [j] == "Smaller of Two Numbers") {
									r = false;
									r_declaration = true;
									evaluateBoolean = true;
								}

								//if value of variable is determined by logical operations
								else if(regexClassification[j] == "Comparing If Equal" || regexClassification[j] == "Comparing If Different"){
									r = false;
									r_declaration = true;
									evaluateComparison = true;
								}

								//if value of variable is not a string
								else if (mapping.ContainsKey (currentVar) && stringChecker == false) {
									if(!skip_statements) mapping [currentVar] = match.Value;
									lex.AppendValues (match.Value, regexClassification [j]);
									lines [i] = lines [i].Remove (0, match.Value.Length);
									lines [i] = lines [i].Trim ();	//to remove trailing spaces
									r = false;
									r_checkerifelse = false;
								} 

								else {
									r = false;
									r_checkerifelse = false;
								}
							}



							//FOR READING KEYWORD 'R'
							else if (regexClassification [j] == "Assignment of Value" && !obtw && !stringChecker && !visibleChecker && !itz_string) {
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
								r = true;
								r_checkerifelse = true;
							}



							//FOR GETTING THE VALUE 'ITZ'
							else if (itz && !r_declaration && !stringChecker && !obtw && !visibleChecker) {

								Console.WriteLine ("current line in itz" + lines [i]);

								//for getting the string
								if (itz_string == true) {

									if (regexClassification [j] == "String Delimiter") {
										itz = false;
										itz_string = false;
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces
										itz_checkerifelse = false;

									} else {
										Console.WriteLine ("inside string == true");
										Console.WriteLine (match.Value);



										var value = mapping [currentVar];
										if (value == "NOOB") {
											if(!skip_statements) mapping [currentVar] = match.Value;
										} else {							
											String stringValue = value + match.Value;
											if(!skip_statements) mapping [currentVar] = stringValue;
										}

										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces
										Console.WriteLine (lines [i]);
										itz_checkerifelse = false;

									}
								}


								//if value of variable is stirng
								else if (regexClassification [j] == "String Delimiter") {
									itz_string = true;
									lines [i] = lines [i].Remove (0, match.Value.Length);
									lines [i] = lines [i].Trim ();	//to remove trailing spaces
									itz_checkerifelse = false;

								}

								//if value of variable is determined by an arithmetic operations
								else if (regexClassification [j] == "Addition" || regexClassification [j] == "Subtraction" || regexClassification [j] == "Multiplication" || regexClassification [j] == "Division" || regexClassification [j] == "Modulo" || regexClassification [j] == "Larger of Two Numbers" || regexClassification [j] == "Smaller of Two Numbers") {
									itz = false;
									itz_declaration = true;
									evaluateArithmetic = true;
								} 


								//if value of variable is determined by boolean operations
								else if (regexClassification [j] == "Logical And" || regexClassification [j] == "Logical Or" || regexClassification [j] == "Logical Xor" || regexClassification [j] == "Logical Not" || regexClassification [j] == "Logical And For Number of Arguments" || regexClassification [j] == "Logical Or For Number of Arguments" || regexClassification [j] == "Smaller of Two Numbers") {
									itz = false;
									itz_declaration = true;
									evaluateBoolean = true;
								}
									
								//if value of variable is determined by logical operations
								else if(regexClassification[j] == "Comparing If Equal" || regexClassification[j] == "Comparing If Different"){
									itz = false;
									itz_declaration = true;
									evaluateComparison = true;
								}


								//if value of variable is not a string
								else if (mapping.ContainsKey (currentVar) && !stringChecker) {
									if(!skip_statements) mapping [currentVar] = match.Value;
									lex.AppendValues (match.Value, regexClassification [j]);
									lines [i] = lines [i].Remove (0, match.Value.Length);
									lines [i] = lines [i].Trim ();	//to remove trailing spaces
									itz = false;
									itz_checkerifelse = false;
								} else {
									itz = false;
									console.Buffer.Text += "ERROR AT " + i + ": Stand alnoe ITZ\n";
									interpreting = false;
									break;
								}
							} 


							//FOR READING KEYWORD 'ITZ'
							else if (regexClassification [j] == "Variable Initialization" && obtw == false && stringChecker == false && visibleChecker == false && itz_string == false) {
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
								itz = true;
								itz_checkerifelse = true;
							}


							//FOR READING KEYWORD "SMOOSH"
							else if(regexClassification [j] == "Yarn Concatenation"){
								smooshChecker = true;

								lex.AppendValues (match.Value, regexClassification [j]);	
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
							}


							//YARN CONCATENATION FOR PUTTING ALL TO STACK
							else if(smooshChecker == true){
								if (btwChecker == true) {
									lex.AppendValues (lines [i], "Comment String");
									lines [i] = "";

									if (lines [i] == "") {
										smooshChecker = false;
										btwChecker = false;

										evaluateSmoosh = true;
									}
								} 

								else {
									if (regexClassification [j] == "Start of Single Line Comment") {
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces
										lex.AppendValues (match.Value, regexClassification [j]);

										btwChecker = true;	
									} else if (regexClassification [j] == "Start of Multi Line Comment") {
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces
										lex.AppendValues (match.Value, regexClassification [j]);

										obtw = true;
										smooshChecker = false;
										evaluateSmoosh = true;

									} else if (regexClassification [j] == "And") {
										lex.AppendValues (match.Value, regexClassification [j]);
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces
									} else {
										operationStack.Push (match.Value);
										lex.AppendValues (match.Value, regexClassification [j]);
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces

										if (lines [i] == "") {
											smooshChecker = false;
											evaluateSmoosh = true;
										}
									}
								}
							}



							//ARITHMETIC OPERATIONS
							else if (regexClassification [j] == "Addition" || regexClassification [j] == "Subtraction" || regexClassification [j] == "Multiplication" || regexClassification [j] == "Division" || regexClassification [j] == "Modulo" || regexClassification [j] == "Larger of Two Numbers" || regexClassification [j] == "Smaller of Two Numbers") {
								//for checking if comparing is present
								if(!comparisonChecker)
									arithmeticChecker = true;

								operationStack.Push (match.Value);
								lex.AppendValues (match.Value, regexClassification [j]);	
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
							} 

							//ARITHMETIC OPERATIONS FOR PUTTING ALL TO STACK
							else if (arithmeticChecker == true) {							

								if (btwChecker == true) {
									lex.AppendValues (lines [i], "Comment String");
									lines [i] = "";

									if (lines [i] == "") {
										arithmeticChecker = false;
										btwChecker = false;

										evaluateArithmetic = true;
									}
								} 

								else {
									if (regexClassification [j] == "Start of Single Line Comment") {
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces
										lex.AppendValues (match.Value, regexClassification [j]);

										btwChecker = true;	
									} else if (regexClassification [j] == "Start of Multi Line Comment") {
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces
										lex.AppendValues (match.Value, regexClassification [j]);

										obtw = true;
										arithmeticChecker = false;
										evaluateArithmetic = true;

									} else if (regexClassification [j] == "And") {
										lex.AppendValues (match.Value, regexClassification [j]);
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces
									} else {
										operationStack.Push (match.Value);
										lex.AppendValues (match.Value, regexClassification [j]);
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces

										if (lines [i] == "") {
											arithmeticChecker = false;
											evaluateArithmetic = true;
										}
									}
								}

							}

						

							//BOOLEAN OPERATIONS
							else if (regexClassification [j] == "Logical And" || regexClassification [j] == "Logical Or" || regexClassification [j] == "Logical Xor" || regexClassification [j] == "Logical Not" || regexClassification [j] == "Logical And For Number of Arguments" || regexClassification [j] == "Logical Or For Number of Arguments") {
								booleanChecker = true;
								operationStack.Push (match.Value);
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces

								if(regexClassification [j] == "Logical And For Number of Arguments" || regexClassification [j] == "Logical Or For Number of Arguments")
									allofanyof = true;
							}


							//BOOLEAN OPERATIONS FOR PUSHING ALL TO STACK
							else if (booleanChecker == true) {

								if (btwChecker == true) {
									lex.AppendValues (lines [i], "Comment String");
									lines [i] = "";

									if (lines [i] == "") {
										booleanChecker = false;
										btwChecker = false;

										evaluateBoolean = true;
									}
								} 

								else {
									if (regexClassification [j] == "Start of Single Line Comment") {
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces
										lex.AppendValues (match.Value, regexClassification [j]);

										btwChecker = true;										
									} else if (regexClassification [j] == "Start of Multi Line Comment") {
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces
										lex.AppendValues (match.Value, regexClassification [j]);

										obtw = true;
										booleanChecker = false;
										evaluateBoolean = true;

									} else if (regexClassification [j] == "And") {
										lex.AppendValues (match.Value, regexClassification [j]);
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces
									} 

									else {
										operationStack.Push (match.Value);
										lex.AppendValues (match.Value, regexClassification [j]);
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces

										if (lines [i] == "") {
											if(allofanyof){
												allofanyof = false;
												String trashholder = (String)operationStack.Pop ();
											}
											Console.WriteLine ("boolean after pushing all to stack");
											booleanChecker = false;
											evaluateBoolean = true;
										}
									}
								}

							}




							//COMPARING OPERATIONS
							else if (regexClassification [j] == "Comparing If Equal" || regexClassification [j] == "Comparing If Different") {
								comparisonChecker = true;
								operationStack.Push (match.Value);
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
							} 

							//COMPARING OPERATIONS FOR PUTTING ALL TO STACK
							else if (comparisonChecker == true) {							

								if (btwChecker == true) {
									lines [i] = lines [i].Remove (0, match.Value.Length);
									lines [i] = lines [i].Trim ();	//to remove trailing spaces
									if (lines [i] == "") {
										comparisonChecker = false;
										btwChecker = false;

										evaluateComparison = true;
									}
								} else {
									if (regexClassification [j] == "Start of Single Line Comment") {
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces
										btwChecker = true;	
									} else if (regexClassification [j] == "Start of Multi Line Comment") {
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces
										obtw = true;
										comparisonChecker = false;
										evaluateComparison = true;

									} else if (regexClassification [j] == "And") {
										lex.AppendValues (match.Value, regexClassification [j]);
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces
									} else {
										operationStack.Push (match.Value);
										lex.AppendValues (match.Value, regexClassification [j]);
										lines [i] = lines [i].Remove (0, match.Value.Length);
										lines [i] = lines [i].Trim ();	//to remove trailing spaces

										if (lines [i] == "") {
											comparisonChecker = false;
											evaluateComparison = true;
										}
									}
								}

							}

							//FOR if-else
							else if (regexClassification [j] == "If Part" && !obtw) {
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();
								if (!orly) {
									console.Buffer.Text += ">ERROR AT " + i + ": O RLY? not found.\n";
									interpreting = false;
									break;
								}

								if (mapping ["IT"] == "FAIL") skip_statements = true; //sets the ya_no flag if it is should proceed to the else part
								j = 0;

							} else if (regexClassification [j] == "Else Part" && !obtw) {
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();
								if (!orly) {
									console.Buffer.Text += ">ERROR AT " + i + ": O RLY? not found." + obtw +"\n";
									interpreting = false;
									break;
								}
								if (mapping ["IT"] == "WIN") skip_statements = true;
								else skip_statements = false;

								j = 0;
							} else if (regexClassification [j] == "Control Statement End" && orly && !obtw) {
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
								orly = false;
								skip_statements = false;
							}
							else if (regexClassification [j] == "If-Else Start" && !obtw) {
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
								orly = true;

								if (mapping ["IT"] != "WIN" && mapping ["IT"] != "FAIL") {
									console.Buffer.Text += ">ERROR: IT must be of TROOF (Boolean) value.\n";
									interpreting = false;
									break;
								}
								interpreting = ya_rly_check (lines, i);
								if (!interpreting) break;
							}
							//END of if-else statments

							//for switch-case
							else if (regexClassification [j] == "Control Statement End" && wtf) {
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
								wtf = false;
								gtfo = false;
								skip_statements = false;
							}

							else if (regexClassification [j] == "Default Switch") {
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces

								if (!wtf) { //if no WTF?
									console.Buffer.Text += "ERROR AT " + i + ": WTF? not found.\n";
									interpreting = false;
									break;
								}

								if (!wtf_match && !gtfo) { //sets the wtf_match flag so it will behave just like having a normal omg match
									wtf_match = true;
									skip_statements = false;
								}
							}
							else if(regexClassification[j] == "Break"){
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
								if (wtf_match) {
									gtfo = true;
									wtf_match = false;
									skip_statements = true;
								}
							}
							else if (regexClassification [j] == "Switch") {
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
								Console.WriteLine ("OMG: " + lines [i]);
								if (!wtf) { //if WTF? is not read but an OMG is read
									console.Buffer.Text += "ERROR AT " + i + ": WTF? not found.\n";
									interpreting = false;
									break;
								}

								//for checking if the line starts with the value of IT variable
								Regex regexCheckingSwitch  = new Regex ("^"+ mapping ["IT"]);
								Match matchCheckingSwitch = regexCheckingSwitch.Match (lines [i]);

								if (!matchCheckingSwitch.Success) {
									incorrect_switch = true;
									if(!wtf_match) skip_statements = true;
								}

								else if(!gtfo) { //else tells that there is a match
									wtf_match = true;
									incorrect_switch = false;
									skip_statements = false;
								}
							}

							else if (regexClassification [j] == "Switch Case Start") { //if WTF? is read
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
								wtf = true;
							}
							//end of switch-case

							//STRINGS
							else if (regexClassification [j] == "String Delimiter" && obtw == false && stringChecker == false && visibleChecker == false) {
								stringChecker = true;
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
							} else if (regexClassification [j] == "String Delimiter" && obtw == false && stringChecker == true && visibleChecker == false) {
								stringChecker = false;
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
							} else if (stringChecker == true && obtw == false && visibleChecker == false) {
								Console.WriteLine (regexClassification [j]);

								mapping [currentVar] = match.Value;


								//lex.AppendValues (match.Value, regexClassification [j]);

								lex.AppendValues (match.Value, "YARN Literal");
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
							}

							else if (regexClassification [j] == "Start of Multi Line Comment" && stringChecker == false && visibleChecker == false) {
								obtw = true;
								//mapping.Add (match.Value, regexClassification [j]);
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
								break;
							}

							//'GIMMEH'
							//Scan Keyword
							else if (regexClassification [j] == "Scan Keyword" && stringChecker == false && visibleChecker == false && gimmeh == false && !skip_statements) {

								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
								CustomInputDialog cid = new CustomInputDialog ();
								if (cid.Run () == (int)Gtk.ResponseType.Ok) {
									gimmehValue = cid.Text;
								}
								cid.Destroy ();

								gimmeh = true;

								j = 0;

							} else if (gimmeh == true) {
								gimmeh = false;
								if (mapping.ContainsKey (match.Value)) {
									mapping [match.Value] = gimmehValue;
									lex.AppendValues (match.Value, regexClassification [j]);

								} 

								//else will prompt error
								else {
									console.Buffer.Text += ">ERROR AT " + (i + 1) + ": Variable '" + match.Value + "' is not yet declared\n";
									interpreting = false;
									break;

								}

							}

							else 
							{
								//mapping.Add (match.Value, regexClassification [j]);
								if (match.Value == "I HAS A ") 
									i_has_a = true;

								if (r==true) 
								{
									if(!incorrect_switch) mapping [currentVar] = match.Value;
									r = false;
								}
								lex.AppendValues (match.Value, regexClassification [j]);
								lines [i] = lines [i].Remove (0, match.Value.Length);
								lines [i] = lines [i].Trim ();	//to remove trailing spaces
							}

							j = 0;
						}										
					}
				}	//end of while lines != ""


				while (operationStack.Count != 0) {
					//EVALUATION OF ARITHMETIC
					if (evaluateArithmetic == true) {							
						Console.WriteLine ("Inside evaluation of arithmetic");

						while (operationStack.Count != 0) {
							Console.Write ("Operation Stack Stack values:");
							PrintValues (operationStack, '\t');

							Console.Write ("Inverse Stack Stack values:");
							PrintValues (inverseStack, '\t');

							Console.Write ("\n");

							String toPush = (String)operationStack.Pop ();								


							if (toPush == "SUM OF " || toPush == "DIFF OF " || toPush == "PRODUKT OF " || toPush == "QUOSHUNT OF " || toPush == "MOD OF " || toPush == "BIGGR OF " || toPush == "SMALLR OF ") {
								//String operation = (String)inverseStack.Pop ();
								String operand1 = (String)inverseStack.Pop ();
								String operand2 = (String)inverseStack.Pop ();

								Console.WriteLine ("operation = " + toPush);
								Console.WriteLine ("operand1 = " + operand1);
								Console.WriteLine ("operand2 = " + operand2);

								//FOR CHECKING IF OPERAND1 IS A FLOAT
								if (mapping.ContainsKey (operand1))
									operand1 = mapping [operand1];

								String operand1Classifier = (String)operand1;
								Regex regexClassifier = new Regex (regexList [0]);
								Match matchClassifier = regexClassifier.Match (operand1Classifier);

								if (matchClassifier.Success)
									isFloat = true;

								if (mapping.ContainsKey (operand2))
									operand2 = mapping [operand2];

								//FOR CHECKING IF OPERAND2 IS A FLOAT
								String operand2Classifier = (String)operand2;
								regexClassifier = new Regex (regexList [0]);
								matchClassifier = regexClassifier.Match (operand2Classifier);


								if (matchClassifier.Success)
									isFloat = true;										

								float sum;
								//FOR CALLING THE CORRECT FUNCTION
								if (toPush == "SUM OF")
									sum = SumOf (float.Parse (operand1), float.Parse (operand2), isFloat);
								else if (toPush == "DIFF OF")
									sum = DiffOf (float.Parse (operand1), float.Parse (operand2), isFloat);
								else if (toPush == "PRODUKT OF")
									sum = ProduktOf (float.Parse (operand1), float.Parse (operand2), isFloat);
								else if (toPush == "QUOSHUNT OF")
									sum = QuoshuntOf (float.Parse (operand1), float.Parse (operand2), isFloat);
								else if (toPush == "MOD OF")
									sum = ModOf (float.Parse (operand1), float.Parse (operand2), isFloat);
								else if (toPush == "BIGGR OF")
									sum = BiggrOf (float.Parse (operand1), float.Parse (operand2), isFloat);

								//else if(toPush == "SMALLR OF")
								else
									sum = SmallrOf (float.Parse (operand1), float.Parse (operand2), isFloat);

								if (isFloat) {
									String pusher1 = Convert.ToString (sum);
									Console.WriteLine ("SUM = " + pusher1);
									inverseStack.Push (pusher1);
									isFloat = false;
								} else {
									String pusher1 = Convert.ToString (sum);
									Console.WriteLine ("SUM = " + pusher1);
									inverseStack.Push (pusher1);
									isFloat = false;
								}									

							} else
								inverseStack.Push (toPush);																		
						}


						if (operationStack.Count == 0) {

							Console.WriteLine ("a");
							evaluateArithmetic = false;

							//FOR VARIABLE DECLARATION WITH OPERATIONS
							if (itz_checkerifelse == true) {
								if (itz_declaration == true) {
									Console.WriteLine ("inside itz checkerifelse");
									if (inverseStack.Count == 1) {
										mapping [currentVar] = (String)inverseStack.Pop ();
										//currentVar = "";
										itz_declaration = false;
										itz_checkerifelse = false;
									}

								} else {
									if (inverseStack.Count == 1) {
										if(!skip_statements) mapping ["IT"] = (String)inverseStack.Pop ();
										//currentVar = "";
										itz_declaration = false;
										itz_checkerifelse = false;
									}
								}
							} 

							else if (r_checkerifelse == true) {
								Console.WriteLine ("inside visible r_checkerifelse");
								Console.WriteLine ("inside visible r_checkerifelse");
								Console.WriteLine ("inside visible r_checkerifelse");
								Console.WriteLine ("inside visible r_checkerifelse");

								if (r_declaration == true) {
									if (inverseStack.Count == 1) {
										if(!skip_statements) mapping [currentVar] = (String)inverseStack.Pop ();
										//currentVar = "";
										r_declaration = false;
										r_checkerifelse = false;
									}

								} else {
									if (inverseStack.Count == 1) {
										if(!skip_statements) mapping ["IT"] = (String)inverseStack.Pop ();
										//currentVar = "";
										r_declaration = false;
										r_checkerifelse = false;
									}
								}
							} 

							else if (visibleIT == true) {
								Console.WriteLine ("inside visible it");
								Console.WriteLine ("inside visible it");
								Console.WriteLine ("inside visible it");
								Console.WriteLine ("inside visible it");
								Console.WriteLine ("inside visible it");
								if(!skip_statements) mapping ["IT"] = (String)inverseStack.Pop ();
								var value = mapping ["IT"];
								if(!skip_statements) console.Buffer.Text += value + "\n";

								visibleIT = false;
							}

							else
								if(!skip_statements) mapping ["IT"] = (String)inverseStack.Pop ();


						}

					}

					//EVALUATION OF BOOLEAN
					else if (evaluateBoolean == true) {
						Console.WriteLine ("Inside evaluation of boolean");

						while (operationStack.Count != 0) {
							Console.Write ("Operation Stack Stack values:");
							PrintValues (operationStack, '\t');

							Console.Write ("Inverse Stack Stack values:");
							PrintValues (inverseStack, '\t');

							Console.Write ("\n");

							String toPush = (String)operationStack.Pop ();
							String stringoperand1;
							String stringoperand2;

							if (toPush == "ALL OF") {
								stringoperand1 = (String)inverseStack.Pop ();
								while (inverseStack.Count != 0) {
									stringoperand2 = (String)inverseStack.Pop ();

									//for converting operand1
									Boolean operand1;
									if (stringoperand1 == "WIN")
										operand1 = true;
									else
										operand1 = false;

									//for converting operand2
									Boolean operand2;
									if (stringoperand2 == "WIN")
										operand2 = true;
									else
										operand2 = false;

									Boolean troof;
									//function call
									troof = AllOf (operand1, operand2);

									if (troof == true)
										stringoperand1 = "WIN";
									else
										stringoperand1 = "FAIL";

								}
								inverseStack.Push (stringoperand1);
							}

							else if (toPush == "ANY OF"){
								stringoperand1 = (String)inverseStack.Pop ();
								while (inverseStack.Count != 0) {
									stringoperand2 = (String)inverseStack.Pop ();

									//for converting operand1
									Boolean operand1;
									if (stringoperand1 == "WIN")
										operand1 = true;
									else
										operand1 = false;

									//for converting operand2
									Boolean operand2;
									if (stringoperand2 == "WIN")
										operand2 = true;
									else
										operand2 = false;

									Boolean troof;
									//function call
									troof = AnyOf (operand1, operand2);

									if (troof == true)
										stringoperand1 = "WIN";
									else
										stringoperand1 = "FAIL";

								}
								inverseStack.Push (stringoperand1);
							}

							else if (toPush == "BOTH OF" || toPush == "EITHER OF" || toPush == "WON OF" || toPush == "NOT" || toPush == "ALL OF" || toPush == "ANY OF") {
								stringoperand1 = (String)inverseStack.Pop ();
								stringoperand2 = "false";

								//because only one operand is needed when "NOT"
								if (toPush != "NOT")
									stringoperand2 = (String)inverseStack.Pop ();									

								Boolean operand1;

								if (stringoperand1 == "WIN")
									operand1 = true;
								else
									operand1 = false;


								Boolean operand2;

								if (stringoperand2 == "WIN")
									operand2 = true;
								else
									operand2 = false;

								Console.WriteLine ("operation = " + toPush);
								Console.WriteLine ("operand1 = " + operand1);
								Console.WriteLine ("operand2 = " + operand2);


								Console.WriteLine ("before map checking");

								//for checking if it is a variable
								if (mapping.ContainsKey (stringoperand1)) {
									if (mapping [stringoperand1] == "WIN")
										operand1 = true;
									else if (mapping [stringoperand1] == "FAIL")
										operand1 = false;
								}

								Console.WriteLine ("after map checking");

								//for checking if it is a variable
								if (mapping.ContainsKey (stringoperand2)) {
									if (mapping [stringoperand2] == "WIN")
										operand2 = true;
									else if (mapping [stringoperand2] == "FAIL")
										operand2 = false;
								}


								Boolean troof;
								//FOR CALLING THE CORRECT FUNCTION
								if (toPush == "BOTH OF")
									troof = BothOf (operand1, operand2);
								else if (toPush == "EITHER OF")
									troof = EitherOf (operand1, operand2);
								else if (toPush == "WON OF")
									troof = WonOf (operand1, operand2);
								//else if (toPush == "NOT")
								else
									troof = NotOf (operand1);							

								String pusher1;
								if (troof == true)
									pusher1 = "WIN";
								else
									pusher1 = "FAIL";
								Console.WriteLine ("troof = " + pusher1);
								inverseStack.Push (pusher1);																	


							} else
								inverseStack.Push (toPush);																		
						}

						if (operationStack.Count == 0) {

							Console.WriteLine ("b");
							evaluateBoolean = false;

							//FOR VARIABLE DECLARATION WITH OPERATIONS
							if (itz_checkerifelse == true) {
								if (itz_declaration == true) {
									Console.WriteLine ("inside itz checkerifelse");
									if (inverseStack.Count == 1) {
										if(!skip_statements) mapping [currentVar] = (String)inverseStack.Pop ();
										//currentVar = "";
										itz_declaration = false;
										itz_checkerifelse = false;
									}

								} else {
									if (inverseStack.Count == 1) {
										if(!skip_statements) mapping ["IT"] = (String)inverseStack.Pop ();
										//currentVar = "";
										itz_declaration = false;
										itz_checkerifelse = false;
									}
								}
							} else if (r_checkerifelse == true) {
								Console.WriteLine ("inside r_checkeriqweqwe");
								Console.WriteLine (currentVar);
								if (r_declaration == true) {
									if (inverseStack.Count == 1) {
										if(!skip_statements) mapping [currentVar] = (String)inverseStack.Pop ();
										//currentVar = "";
										r_declaration = false;
										r_checkerifelse = false;
									}

								} else {
									if (inverseStack.Count == 1) {
										if(!skip_statements) mapping ["IT"] = (String)inverseStack.Pop ();
										//currentVar = "";
										r_declaration = false;
										r_checkerifelse = false;
									}
								}
							} 

							else if (visibleIT == true) {
								if(!skip_statements) mapping ["IT"] = (String)inverseStack.Pop ();

								var value = mapping ["IT"];
								if(!skip_statements) console.Buffer.Text += value + "\n";

								visibleIT = false;
							}

							else
								if(!skip_statements) mapping ["IT"] = (String)inverseStack.Pop ();

						}

					}
				
					//EVALUATION OF COMPARISON
					else if (evaluateComparison == true) {
						Console.WriteLine ("Inside evaluation of comparison");

						while (operationStack.Count != 0) {
							Console.Write ("Operation Stack Stack values:");
							PrintValues (operationStack, '\t');

							Console.Write ("Inverse Stack Stack values:");
							PrintValues (inverseStack, '\t');

							Console.Write ("\n");

							String toPush = (String)operationStack.Pop ();
							Console.WriteLine ("toPushQWEQEWQWEQWEQWEQWE = " + toPush);
							String operand1;
							String operand2;

							if (toPush == "BOTH SAEM" || toPush == "DIFFRINT" || toPush == "BIGGR OF" || toPush == "SMALLR OF") {
									operand1 = (String)inverseStack.Pop ();
									operand2 = (String)inverseStack.Pop ();

									Console.WriteLine ("operation = " + toPush);
									Console.WriteLine ("operand1 = " + operand1);
									Console.WriteLine ("operand2 = " + operand2);

									//FOR VARIABLES
									if (mapping.ContainsKey (operand1))
									operand1 = mapping [operand1];

									if (mapping.ContainsKey (operand2))
									operand2 = mapping [operand2];

									//if to push is biggr of or smallr of
									if (toPush == "BIGGR OF" || toPush == "SMALLR OF") {

										//FOR CHECKING IF OPERAND1 IS A FLOAT
										if (mapping.ContainsKey (operand1))
											operand1 = mapping [operand1];

										String operand1Classifier = (String)operand1;
										Regex regexClassifier = new Regex (regexList [0]);
										Match matchClassifier = regexClassifier.Match (operand1Classifier);

										if (matchClassifier.Success)
											isFloat = true;

										//FOR CHECKING IF OPERAND2 IS A FLOAT
										if (mapping.ContainsKey (operand2))
											operand2 = mapping [operand2];

										String operand2Classifier = (String)operand2;
										regexClassifier = new Regex (regexList [0]);
										matchClassifier = regexClassifier.Match (operand2Classifier);


										if (matchClassifier.Success)
											isFloat = true;

										float result;

										if(toPush == "BIGGR OF")
											result = BiggrOf (float.Parse (operand1), float.Parse (operand2), isFloat);

										//if toPush == "SMALLR OF"
										else
											result = SmallrOf (float.Parse (operand1), float.Parse (operand2), isFloat);

										if (isFloat) {
											String pusher1 = Convert.ToString (result);
											Console.WriteLine ("result = " + pusher1);
											inverseStack.Push (pusher1);
											isFloat = false;
										} 

										else {
											String pusher1 = Convert.ToString (result);
											Console.WriteLine ("result = " + pusher1);
											inverseStack.Push (pusher1);
											isFloat = false;
										}
										
									}

									else if(toPush == "BOTH SAEM" || toPush == "DIFFRINT"){								

										Console.WriteLine ("INSIDE BOTH SAEM AND DIFFRINT");				

										//FOR VARIABLES
										if (mapping.ContainsKey (operand1))
											operand1 = mapping [operand1];

										if (mapping.ContainsKey (operand2))
											operand2 = mapping [operand2];									

										Boolean cmp;
										//FOR CALLING THE CORRECT FUNCTION
										if (toPush == "BOTH SAEM")
											cmp = BothSaem (operand1, operand2);

										//else if(toPush == "DIFFRINT")
										else
											cmp = Diffrint (operand1, operand2);

										String pusher1;
										if (cmp == true)
											pusher1 = "WIN";
										else
											pusher1 = "FAIL";


										inverseStack.Push (pusher1);

									}

								} 

								else
									inverseStack.Push (toPush);																		
							}

								if (operationStack.Count == 0) {

									Console.WriteLine ("b");
									evaluateBoolean = false;

									//FOR VARIABLE DECLARATION WITH OPERATIONS
									if (itz_checkerifelse == true) {
										if (itz_declaration == true) {
											Console.WriteLine ("inside itz checkerifelse");
											if (inverseStack.Count == 1) {
												if(!skip_statements) mapping [currentVar] = (String)inverseStack.Pop ();
												//currentVar = "";
												itz_declaration = false;
												itz_checkerifelse = false;
											}

										} else {
											if (inverseStack.Count == 1) {
												if(!skip_statements) mapping ["IT"] = (String)inverseStack.Pop ();
												//currentVar = "";
												itz_declaration = false;
												itz_checkerifelse = false;
											}
										}
									} else if (r_checkerifelse == true) {
										Console.WriteLine ("inside r_checkeriqweqwe");
										Console.WriteLine (currentVar);
										if (r_declaration == true) {
											if (inverseStack.Count == 1) {
												if(!skip_statements)  mapping [currentVar] = (String)inverseStack.Pop ();
												//currentVar = "";
												r_declaration = false;
												r_checkerifelse = false;
											}

										} else {
											if (inverseStack.Count == 1) {
												if(!skip_statements) mapping ["IT"] = (String)inverseStack.Pop ();
												//currentVar = "";
												r_declaration = false;
												r_checkerifelse = false;
											}
										}
									} 

									else if (visibleIT == true) {
										if(!skip_statements) mapping ["IT"] = (String)inverseStack.Pop ();

										var value = mapping ["IT"];
										if(!skip_statements) console.Buffer.Text += value + "\n";

										visibleIT = false;
									}

									else
										if(!skip_statements) mapping ["IT"] = (String)inverseStack.Pop ();

								}

								}
				
					//EVALUATION OF STRING CONCAT
					else if(evaluateSmoosh == true){

						Console.WriteLine ("Inside evaluation of smoosh");

						while (operationStack.Count != 0) {
							Console.Write ("Operation Stack Stack values:");
							PrintValues (operationStack, '\t');

							Console.Write ("Inverse Stack Stack values:");
							PrintValues (inverseStack, '\t');

							Console.Write ("\n");

							String toPush = (String)operationStack.Pop ();


							Console.WriteLine ("toPush = " + toPush);


							//FOR VARIABLES
							if (mapping.ContainsKey (toPush))
								toPush = mapping [toPush];


							Console.WriteLine ("after mapping toPush = " + toPush);

							toPush = toPush + smoosh;							
							smoosh = toPush;	

						}

						Console.WriteLine ("smoosh after looping" + smoosh);
						Console.WriteLine ("operationstack.Count =" + operationStack.Count);
						if (operationStack.Count == 0) {

							Console.WriteLine("inside operation stack.count == 0");
							evaluateSmoosh = false;

							//FOR VARIABLE DECLARATION WITH OPERATIONS
							if (itz_checkerifelse == true) {
								Console.WriteLine ("inside itz_checkerifelse");
								if (itz_declaration == true) {
									Console.WriteLine ("inside itz checkerifelse");
										if (!skip_statements)
											mapping [currentVar] = smoosh;
										//currentVar = "";
										itz_declaration = false;
										itz_checkerifelse = false;

								} else {
										if (!skip_statements)
											mapping ["IT"] = smoosh;
										//currentVar = "";
										itz_declaration = false;
										itz_checkerifelse = false;
								}
							} 

							else if (r_checkerifelse == true) {
								Console.WriteLine ("inside r_checkeriqweqwe");
								Console.WriteLine (currentVar);
								if (r_declaration == true) {
										if (!skip_statements)
											mapping [currentVar] = smoosh;
										//currentVar = "";
										r_declaration = false;
										r_checkerifelse = false;

								} else {
										if (!skip_statements)
											mapping ["IT"] = smoosh;
										//currentVar = "";
										r_declaration = false;
										r_checkerifelse = false;
								}
							} 

							else if (visibleIT == true) {
								Console.WriteLine ("inside visibleIT");
								if (!skip_statements)
									mapping ["IT"] = smoosh;

								var value = mapping ["IT"];
								if (!skip_statements)
									console.Buffer.Text += value + "\n";

								visibleIT = false;
							} 


							else
								if(!skip_statements) mapping ["IT"] = smoosh;
								Console.WriteLine ("mapping of it");
								

						}


					}
					
		
				}	//end of evaluation of stack

			}
		}


		// TO BE EDITED
		if (wtf || orly) //ADDED: to check if control statements are properly closed
			console.Buffer.Text += ">ERROR AT " + i + ": OIC expected.\n";
		else if (obtw) //ADDED: to check if multi line comments are properly closed
			console.Buffer.Text += ">ERROR AT " + i + ": TLDR expected.\n";
		else if(interpreting)
			console.Buffer.Text += ">Code interpretted!\n"; //CHANGED: added \n at start
		//resets symbol table

		foreach (TreeViewColumn col in symbol.Columns) {
			symbol.RemoveColumn (col);
		}

		symbol.AppendColumn (modColumn);
		symbol.AppendColumn (valColumn);


		foreach (string key in mapping.Keys) {
			var value = mapping [key];
			sym.AppendValues (key, value);
		}

		printAllBooleans (mapping["IT"], visibleIT);



	}

	void PrintValues( IEnumerable myCollection, char mySeparator )  {
		foreach ( object obj in myCollection )
			Console.Write( "{0}{1}", mySeparator, obj );
		Console.WriteLine();
	}

	//SUM OF FUNCTION
	float SumOf (float operand1, float operand2, Boolean isFloat){
		if (isFloat) {
			float sum = operand1 + operand2;
			return (float)sum;
		}

		else {
			int sum = (int)operand1 + (int)operand2;
			return (int)sum;
		}
	}

	//DIFF OF FUNCTION
	float DiffOf (float operand1, float operand2, Boolean isFloat){
		if (isFloat) {
			float difference = operand1 - operand2;
			return (float)difference;
		}

		else {
			int difference = (int)operand1 - (int)operand2;
			return (int)difference;
		}
	}

	//PRODUKT OF FUNCTION
	float ProduktOf (float operand1, float operand2, Boolean isFloat){
		if (isFloat) {
			float product = operand1 * operand2;
			return (float)product;
		}

		else {
			int product = (int)operand1 * (int)operand2;
			return (int)product;
		}
	}

	//QUOSHUNT OF FUNCTION
	float QuoshuntOf (float operand1, float operand2, Boolean isFloat){
		if (isFloat) {
			float quotient = operand1 / operand2;
			return (float)quotient;
		}

		else {
			int quotient = (int)operand1 / (int)operand2;
			return (int)quotient;
		}
	}

	//MOD OF FUNCTION
	float ModOf (float operand1, float operand2, Boolean isFloat){
		if (isFloat) {
			float modulo = operand1 % operand2;
			return (float)modulo;
		}

		else {
			int modulo = (int)operand1 % (int)operand2;
			return (int)modulo;
		}
	}

	//BIGGR OF FUNCTION
	float BiggrOf (float operand1, float operand2, Boolean isFloat){
		if (isFloat) {
			//float modulo = operand1 % operand2;
			if (operand1 > operand2)
				return (float)operand1;
			else if (operand2 > operand1)
				return (float)operand2;
			else
				return (float)operand1;
		}

		else {
			if ((int)operand1 > (int)operand2)
				return (int)operand1;
			else if ((int)operand2 > (int)operand1)
				return (int)operand2;
			else
				return (int)operand1;
		}
	}

	//SMALLR OF FUNCTION
	float SmallrOf (float operand1, float operand2, Boolean isFloat){
		if (isFloat) {
			//float modulo = operand1 % operand2;
			if (operand1 < operand2)
				return (float)operand1;
			else if (operand2 < operand1)
				return (float)operand2;
			else
				return (float)operand1;
		}

		else {
			if ((int)operand1 < (int)operand2)
				return (int)operand1;
			else if ((int)operand2 < (int)operand1)
				return (int)operand2;
			else
				return (int)operand1;
		}
	}



	Boolean BothOf(Boolean operand1, Boolean operand2){
		if (operand1 == true && operand2 == true)
			return true;
		else 
			return false;
	}

	Boolean EitherOf(Boolean operand1, Boolean operand2){
		if (operand1 == true || operand2 == true)
			return true;
		else 
			return false;
	}

	Boolean WonOf(Boolean operand1, Boolean operand2){
		if (operand1 == operand2)
			return false;
		else 
			return true;
	}

	Boolean NotOf(Boolean operand1){
		return !operand1;
	}

	Boolean AllOf(Boolean operand1, Boolean operand2){
		if (operand1 == true && operand2 == true)
			return true;
		else 
			return false;
	}

	Boolean AnyOf(Boolean operand1, Boolean operand2){
		if (operand1 == true || operand2 == true)
			return true;
		else 
			return false;
	}

	Boolean BothSaem(String operand1, String operand2){
		return operand1.Equals (operand2);
	}

	Boolean Diffrint(String operand1, String operand2){
		return !(operand1.Equals (operand2));
	}

	void printAllBooleans(String name, Boolean flag){
		Console.WriteLine (name + " " + flag);
	}

	Boolean ya_rly_check(String[] lines, int i){ //ADDED: this whole function
		Regex ya_rly = new Regex ("^YA RLY");
		Regex comment_start = new Regex ("^O?BTW");

		while (ya_rly.Match (lines [i]).Success && i <= lines.Length - 1) {
			i++;
			lines [i] = lines [i].Trim ();
			if (i == lines.Length - 1 && !comment_start.Match(lines[i]).Success) {
				console.Buffer.Text += "ERROR AT" + i + ": Expecting YA RLY\n";
				return false;
			}
		}

		return true;
	}

}


