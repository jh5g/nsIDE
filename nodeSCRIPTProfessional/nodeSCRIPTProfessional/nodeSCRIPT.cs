using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using nsNodes;
using nsAvancedMathematics;

namespace nodeSCRIPTProfessional
{
    class ID
    {
        public List<IDs> allIDsPerm { get; set; }
        public ID()
        {
            this.allIDsPerm = new List<IDs>();
        }
    }
    class IDs
    {
        public List<Instruction> contents { get; set; }
        public List<int> ID { get; set; }
        public int state { get; set; }
        public IDs()
        {
            this.ID = new List<int>();
            this.state = 0;
            this.contents = new List<Instruction>();
        }
        public List<IDs> GetAllIDs(ID group)
        {
            return group.allIDsPerm;
        }
        public void IDGive(int newIDState, ID group)
        {
            List<IDs> allIDsPermLocal = GetAllIDs(group);
            List<List<int>> listOfIDs = new List<List<int>>();
            List<int> lastParent = new List<int>();
            List<int> newID = new List<int>();
            List<int> mostRecentOpenID = new List<int>();
            bool OpenIDExists = false;
            // Check that an open ID exists
            foreach (IDs thing in allIDsPermLocal)
            {
                if (thing.state == 1)
                {
                    OpenIDExists = true;
                    mostRecentOpenID = thing.ID;
                }
                listOfIDs.Add(thing.ID);
            }
            if (allIDsPermLocal.Count != 0 && OpenIDExists)
            {
                newID = mostRecentOpenID.ToList<int>();
                newID.Add(0); //This used to also change mostRecentOpenID (.ToList fixes this)
                foreach (List<int> comparisonIDList in listOfIDs)  // if this ID already exists new one is parralel to it, add one to final number
                {
                    string comparisonIDListString = string.Join(",", comparisonIDList);
                    string newIDString = string.Join(",", newID);
                    while (comparisonIDListString == newIDString)
                    {
                        newID[newID.Count - 1] += 1;
                        newIDString = string.Join(",", newID);
                    }
                }
                this.ID = newID;
                this.state = newIDState;
                group.allIDsPerm.Add(this);
            }
            else if (allIDsPermLocal.Count != 0) //&& !! OpenIDExists
            {
                foreach (IDs thing in allIDsPermLocal)
                {
                    if (thing.ID.Count == 1)
                    {
                        lastParent = thing.ID;
                    }
                }
                newID = lastParent.ToList<int>();
                newID[0] += 1; //If none of the IDs are open this must be a parent so it is parralel to the last parent
                this.ID = newID;
                this.state = newIDState;
                group.allIDsPerm.Add(this);
            }
            else
            {
                newID.Add(0); // First ID must be ID 0
                this.ID = newID;
                this.state = newIDState;
                group.allIDsPerm.Add(this);
            }
        }
        public IDs MostRecentOpen(ID group)
        {
            List<IDs> allIDsPermLocal = new List<IDs>();
            IDs mostRecentOpenID = new IDs();
            allIDsPermLocal = group.allIDsPerm.ToList<IDs>();
            foreach (IDs thing in allIDsPermLocal)
            {
                if (thing.state == 1)
                {
                    mostRecentOpenID = thing;
                }
            }
            return mostRecentOpenID;
        }
        public void IDClose(ID group)
        {
            MostRecentOpen(group).state = 0;
        }
        public List<Instruction> GetInstructions()
        {
            return this.contents;
        }
        public void AddInstruction(Instruction toAdd)
        {
            this.contents.Add(toAdd);
        }
        public string IDString()
        {
            string IDOut = "";
            foreach (int content in this.ID)
            {
                IDOut += content.ToString();
                IDOut += ",";
            }
            return IDOut;
        }
    }

    class Utils
    {
       public List<char> symbols { get; set; }

        //  = { '=', '+', '/', '*', '%' }; // All the symbols potentially used in an expression

       public char[] lineDefineSymbols { get; set;  }

        // { ';', '{', '}' }; // All the symbols at the end of a line that define it as a statement or conductor

        public Utils()
        {
            this.symbols = new List<char>();
            char[] symbols = { '=', '+', '/', '*', '%', ';'};

            this.symbols.AddRange(symbols);
            

        }

        public static void ErrorReport(string error, string message, string style)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[" + style + "]");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" An error occurred while running nodeSCRIPTProfessional:");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\nError type: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(error);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\nDescription: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (style != "WARN")
            {
                Console.WriteLine(message + "\n\nPress any key to exit the program.");
            }
        }
    }


    class Function
    {
        public List<string> Args { get; set; }
        public string Contents { get; set; }
        public string Instructions { get; set; }
        public string Name { get; set; }
        // public bool State { get; set; }

        public Function(string arguments, string name)
        {
            this.Name = name;
            
        }

        public void EndFunction(string contents)
        {
            this.Contents = contents;

        }

    }

    
    class Expression
    {
        public string ExpressionType { get; set; }
        public string FinalValue { get; set; }

        public Expression(string Expr)
        {
            Utils utility = new Utils();
            List<string> allItems = new List<string>();
            string currentPhrase = "";
            bool stringOpenState = false;
            Expr += ";"; // Just to mark the end of the expression, as the ; was stripped away when the line was first recognised

            foreach (char character in Expr)
            {
                if (character == '"')
                {
                    if (stringOpenState == false)
                    {
                        currentPhrase += "\"";
                        stringOpenState = true;
                    }
                    else
                    {
                        currentPhrase += "\"";
                        stringOpenState = false;
                    }
                }
                else if (utility.symbols.Contains(character) && stringOpenState != true)
                {
                    allItems.Add(currentPhrase);
                    currentPhrase = "";
                }
                else
                {
                    currentPhrase += character;
                }
            }

            Console.WriteLine("ALL ITEMS IN EXPRESSION: {0}", String.Join(", ", allItems));



            foreach (string item in allItems)
            {
                Console.WriteLine("ITEM: {0}", item);



                string strippedItem = (item.Replace("\t", "")).Replace(" ", "");

                /* Commented out as it doesn't work for some reason
                Match isString = Regex.Match(item, "\\s+(\\\")(.*?)(\\\")\\s+");
                if (isString.Success)
                {
                    Console.WriteLine("STRING FOUND");
                }
                */
                

                Match isInt = Regex.Match(strippedItem, @"\d");
                if (isInt.Success)
                {
                    Console.WriteLine("IS INT: {0}", strippedItem);
                }

            }
        }

        public string GetNetType(string expr)
        {
            return "";
        }

        public string GetFinalType(string item)
        {
            //TODO: string, int, float, *
            string token = "";

            int notString = 0; // 0 Means it has not been set, 1 means it is true, 2 means it is false

            bool stringState = false;

            foreach (char character in item)
            {
                token += character;
                if (token != "" && token != " " && !!stringState)
                {
                    // It is not a string.
                    notString = 1;
                }
                else if (token == "\"")
                {
                    if (notString == 0)
                    {
                        notString = 1;
                    }
                }
            }

            return "";
        }

          
    }

    class InstructionSet
    {
        public static List<Instruction> Instructions { get; set; }


        public InstructionSet()
        {
            Instructions = new List<Instruction>();
        }

        public Instruction createInstruction(string line)
        {
            Instruction currentInstruction = new Instruction(line);
            Instructions.Add(currentInstruction);
            return currentInstruction;
        }

        public List<Instruction> getInstructions()
        {
            return Instructions;
        }
    }

    class Instruction
    {
        public string InstructionType { get; set; } // We are going to use these later
        public string Contents { get; set; }
        public static ID curleyBrackets = new ID();
        public static ID normalBrackets = new ID();
        public static ID squareBrackets = new ID();
        public static Dictionary<IDs, List<object>> curleyContents = new Dictionary<IDs, List<object>>();
        public static Dictionary<IDs, List<object>> normalContents = new Dictionary<IDs, List<object>>();
        public static Dictionary<IDs, List<object>> squareContents = new Dictionary<IDs, List<object>>();
        public static int instructionNumber = 0;
        public IDs ID { get; set; }



        string lineToParse = ""; // The current line to make sense of

        //                                                                                                                                        ||
        //                                                                                                                                        \/
        public Instruction(string instruction_string) // This is initialised upon creation of the object, Instruction testInstruction = new Instruction("test;");
        {
            instructionNumber += 1;
            this.InstructionType = "null"; // In case none of the following IFs are true
            
            string last_char = (instruction_string.Substring(instruction_string.Length - 1)); // Last character of the string
            if (last_char.Equals(";"))
            {
                this.InstructionType = "statement";
            }
            if (last_char.Equals("{") || last_char.Equals("}"))
            {
                IDs tempID = new IDs(); // Init obj before use so that we can use it in both cases for open & close
                if (last_char.Equals("{"))
                {
                    // Open new ID
                    tempID.IDGive(1, curleyBrackets);
                    this.ID = tempID;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("OPEN: {0}", tempID.IDString());
                    Console.ForegroundColor = ConsoleColor.Gray;

                }
                else
                {
                    tempID.IDClose(curleyBrackets);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("PREVIOUS CLOSED, NOW ON: {0}", tempID.MostRecentOpen(curleyBrackets).IDString());
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                this.InstructionType = "conductor";
            }
            this.Contents = instruction_string;
            lineToParse = this.Contents; // Instruction is one line, so all of it is the line to parse
        }

        public List<object> ParseInstruction() // Function for outputting a list of what the instruction does
        {
            
            // [--------------------] Variable Declarations [--------------------]

            // Lists {
            List<object> finalInstructions = new List<object>(); // List of instructions to be returned
            // } Strings {

            // } Bools & States {

            // }

            // [--------------------] End of Variable Declarations [--------------------]

            // [--------------------] Horrible regex matching for syntax [--------------------]
            /*
             * Notes:
             * 
             * - Anything in ( ) [NOT \( or \) ] is just a fixed part of the string, e.g (if) in if(conditon){ is referring to the word "if" as it never changes in an if statement
             * - \s+ is all whitespace in that area
             * - (.*) or (.*?) is a placeholder for anything until the next placeholder
             * - (\w+) is usually just a word or name for something, e.g a node name
             *  
             */

            Match nodeDeclarationMatches = Regex.Match(lineToParse.Remove(lineToParse.Length - 1), "");

            Match varDeclarationMatches = Regex.Match(lineToParse.Remove(lineToParse.Length -1), @"(var)\s+(\w+)@(\w+)\s+(=)\s+(.*)"); // This looks horrible, but this is what it means:
            /*
             *  (var)\s+(\w+)@(\w+)\s+(=)\s+(.*)
             *  "var"  | str'@'str  | '=' | anything after
             *     whitespace     whitespace
             */

            Match ifStatementMatches = Regex.Match((lineToParse.Replace(" ", "")).Replace("\t", ""), @"(if)\((.*?)\)({)"); // This looks horrible, but this is what it means:
            /*
             * @"(if)\((.*?)\)({)"
             *   str |   |   |"{"
             *      "("  |  ")"
             *        Anything
             * NOTE: The last character of lineToParse is not removed to check that it is actually an if statement, as { has to be on the end.
             */
            
            Match callingFunctionMatches = Regex.Match(lineToParse.Remove(lineToParse.Length - 1), @"(\w+)\((.*?)\)"); // This, yet again, looks horrible. Here's what it means:
                                                                                                                       /*
                                                                                                                        * @"(\w+)\((.*?)\)"
                                                                                                                        *     |  "(" |  ")"
                                                                                                                        *   a str    |
                                                                                                                        *         Anything
                                                                                                                        * 
                                                                                                                        */




            // [--------------------] End of horrible regex for syntax [--------------------]

            // [--------------------] Checks of syntax [--------------------]

            bool foundMatch = false;

            if (varDeclarationMatches.Success)
            {
                foundMatch = true;
                // The current line is a variable declaration!
                Console.WriteLine("VAR DECLARATION: {0}\n VAR NAME: {1}\n VAR NODE: {2}\n VAR EXPR: {3}", lineToParse, varDeclarationMatches.Groups[2], varDeclarationMatches.Groups[3], varDeclarationMatches.Groups[5]);
                string varName = varDeclarationMatches.Groups[2].ToString();
                string varNode = varDeclarationMatches.Groups[3].ToString();
                Expression varExpression = new Expression(varDeclarationMatches.Groups[5].ToString());
                string varValue = varExpression.FinalValue;
                List<object> indivualInstructions = new List<object>();
                indivualInstructions.Add("VAR");
                indivualInstructions.Add(varName);
                indivualInstructions.Add(varNode);
                indivualInstructions.Add(varValue);
                finalInstructions.Add(indivualInstructions);
            }
            else if (ifStatementMatches.Success)
            {
                foundMatch = true;
                Console.WriteLine("FOUND MATCH {0}", foundMatch);
                // The current line opens an if statement
                Console.WriteLine("IF STATEMENT: {0}\n IF CONDITION: {1}", lineToParse, ifStatementMatches.Groups[2]);
            }
            else if (callingFunctionMatches.Success)
            {
                foundMatch = true;
                // The current line calls a function.
                Console.WriteLine("FUNCTION CALL: {0}\nFUNCTION NAME: {1}\nFUNCTION ARGS: {2}", lineToParse, callingFunctionMatches.Groups[1].ToString(), callingFunctionMatches.Groups[2]);
            }
            Console.WriteLine(foundMatch);
            string instructionLine = lineToParse;

            if (!! foundMatch)
            {
                Console.WriteLine("LINE: {0}", instructionLine);
                Console.WriteLine("UNIDENTIFIED");
                //  string errorMessage = "Skipping unidentified instruction: \"{0}\" (have you checked your spelling?) @ instruction {1}", lineToParse, instructionNumber;
                // Utils.ErrorReport("unidentified_instruction", "Skipping unidentified instruction: " + lineToParse + " (have you checked your spelling?) @ instruction ", "WARN");
            }




            /*
            foreach (char character in lineToParse)
            {
                token += character; // Add the character to current phrase
                
                if (character == ' ' || character == '\t') // If it is a space or tab
                {
                    // TODO: Check if we're in a string declaration
                    token = token.Remove(token.Length - 1); // Remove the last character of Token (the space)
                } // The next function after this should start with if (not else if) as this is global.

                // [---------------] Phrase checkers [---------------]
                
                if (token == "var") // If a var declaration has begun
                {
                    token = ""; // Resets the current phrase as we don't need the static word "var"
                    expectVarName = true;
                }

                // [---------------] End of Phrase checkers [---------------]


                // [---------------] String collectors [---------------]

                else if (expectVarName) // If we are collecting the name of a variable
                {
                    if (character == '=') // If we have finished saying the name@node
                    {
                        string varName = token.Remove(token.Length - 1); // varName is the name of the var without the '=' on the end
                        expectVarName = false; // We are no longer collecting a name
                        expectVarValue = true;
                    }
                }

                else if (expectVarValue) // If we are collecting the value of a variable
                {
                    if (character == ';') // If it is the end of the variable declaration
                    {
                        string varValue = token.Remove(token.Length - 1); // Set the value of the var to everything without the ';' on the end

                    }
                }
            }
            */

            foreach (List<object> instruction in finalInstructions)
            {
                Console.Write("[");
                foreach (var elem in instruction)
                {
                    Console.Write("{0}, ", elem);
                }
                Console.WriteLine("]");
            }
            return finalInstructions;
        }

        public void AddID(List<object> FinalInstructions)
        {
            IDs temporaryPlaceholderID = new IDs(); // Just a temporary ID to access the most recent ID function, it is not added to anything.
            curleyContents[temporaryPlaceholderID.MostRecentOpen(curleyBrackets)] = FinalInstructions; // Add the final instructions of that line to the dict with a key of the ID of the block (set of { } ) it is in.
        }
    }

    class nodeSCRIPT
    {
        [DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);


        static void Main(string[] args)
        {



            // [-----------] File Association Section (.ns) [-----------]

            if (!! isAssociated())
            {
                Associate(); // If it isn't, associate it
            }

            // [-----------] End of File Association [-----------]

            if (args.Length > 0)
            {
                string ns_file = args[0]; // Gets the first argument given by the system after nodeSCRIPTProfessional.exe
                Run(ns_file); // Sends the file to be read and compiled
            } else
            {
                //TODO: Launch shell
                //TODO: Launch IDE?
                Console.Write("Please enter a file to run: ");
                Run(Console.ReadLine());
                Console.ReadKey();
            }
        }


        public static bool isAssociated()
        {
            return (Registry.CurrentUser.OpenSubKey("Software\\Classes\\.ns", false) == null);
        }

        public static void Associate()
        {
            RegistryKey FileReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\.ns");
            RegistryKey AppReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\Applications\\nodeSCRIPTProfessional.exe");
            RegistryKey AppAssoc = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExtszz.ns");
            FileReg.CreateSubKey("DefaultIcon").SetValue("", Directory.GetCurrentDirectory() + "\\nodeSCRIPT.ico");
            FileReg.CreateSubKey("PerceivedType").SetValue("", "nodeSCRIPT Executable");

            AppReg.CreateSubKey("shell\\open\\command").SetValue("", "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\" %1");
            // TODO: IDLE Editor Path
            // AppReg.CreateSubKey("shell\\edit\\command").SetValue("", "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\" %1");

            AppAssoc.CreateSubKey("UserChoice").SetValue("Progid", "Applications\\nodeSCRIPTProfessional.exe");

            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);

        }

        public static void Run(string file_to_run)
        {
            try
            {
                // Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(file_to_run);

                // Read the first line of text
                string current_line = sr.ReadLine();

                string all_lines = ""; // Every line of the file

                while (current_line != null) // Continue to read until you reach end of file (current_line == null)
                {
                    all_lines += current_line + "\n"; // Adds the line to the string of the file

                    current_line = sr.ReadLine(); // Read the next line
                }
                all_lines += "<EOF>;"; // Adds a marker for showing the end of the file to the compiler
                // Console.WriteLine("File contents: \n");
                // Console.WriteLine(all_lines + "\n");

                sr.Close(); // Close the file stream
                Compile(all_lines);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nProgram finished with no errors. Press any key to exit.");

                Console.ReadKey();
            }
            catch (FileNotFoundException exception)
            {
                Utils.ErrorReport("file_not_found", "The file you have entered does not exist.", "FATAL");
            }
            catch (ArgumentException exception)
            {
                Utils.ErrorReport("invalid_arguments", "The filename you have entered is invalid.", "FATAL");
            }
        }
        public static void Compile(string file_contents)
        {
            var raw_instructions = new List<string>();
            raw_instructions = AnalyseComponent(file_contents);
            /*
            var lines = "[" + string.Join(", ", raw_instructions.ToArray()) + "]";
            lines = lines.Replace("\n", "");
            Console.WriteLine(lines);
            */

            List<object> finalInstructions = CreateInstructions(raw_instructions);

        }
        
        public static List<string> AnalyseComponent(string component) // Returns the statements and conductors of the component
        /*
        main(){                        <---- Conductor
            say("Hello", "World!");    <---- Statement
        }                              <---- Conductor

        Anything that is showing the opening or closing of a clause is a conductor. This includes:
            - If statements (yes I know they're called statements but shush, they open a clause)
            - Function declarations
            - Node declarations
            - Anything with { and }
         */
        {
            var statements = new List<string>();
            string current_token = "";
            int conductor_level = 0;
            foreach(char character in component)
            {
                current_token += character;
                if (character == ';' || character == '{' || character == '}')
                {
                    statements.Add((current_token.Replace("\t", "")).Replace("\n", ""));
                    if (character == '{')
                    {
                        conductor_level += 1;
                    }
                    else if (character == '}')
                    {
                        if (conductor_level <= 0)
                        {
                            Utils.ErrorReport("unbalanced_cbrackets", "Brackets do not line up; too many closing curley brackets", "FATAL");
                            Console.ReadKey();
                            System.Environment.Exit(1);
                        }
                        conductor_level -= 1;
                    }
                    current_token = "";
                }
            }


            return statements;
        }

        public static List<object> CreateInstructions(List<string> raw_instructions)
        {
            InstructionSet Instructions = new InstructionSet();



            foreach (string line in raw_instructions)
            {
                Instruction currentInstruction = Instructions.createInstruction(line);
                // Console.WriteLine("Instruction: {0} = {1}", instruction.Contents, instruction.InstructionType);
                currentInstruction.AddID(currentInstruction.ParseInstruction());
            }



            return null;
        }
    }
}


/*
 * public static string RunThrough(List<string> toRun)
{
    string endResult = "";
    bool inBrackets = false;
    List<string> bracketContents = new List<string>();
    int bracketLevel = 0;
    List<string> finalItems = new List<string>();

    foreach (string item in toRun)
    {
        if (item == "(")
        {
            if (inBrackets == false)
            {
                inBrackets = true;
            }
            else
            {
                bracketLevel += 1;
            }
        }
        else if (item == ")")
        {
            if (bracketLevel > 0)
            {
                bracketLevel -= 1;
            }
            else
            {
                inBrackets = false;
                finalItems.Add(RunThrough(bracketContents)); // Compute the final value of what's in the brackets before doing anything else
            }
        }
        else if (inBrackets == true)
        {
            bracketContents.Add(item);
        }
        else if 


    }

    return endResult;
}
*/