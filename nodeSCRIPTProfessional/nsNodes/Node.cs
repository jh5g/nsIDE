using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nsNodes
{
    public class Node
    {
        public string NodeType { get; set; }
        public string Contents { get; set; }
        public List<string> Instructions { get; set; }
        public Dictionary<string, string> Variables = new Dictionary<string, string>(); // Variables example {"testVar", "int:12"} this would be called as testNode.Variables["testVar"]

        static Dictionary<string, Node> allNodes = new Dictionary<string, Node>(); // All nodes example: {"testNode": testNode}

        public Node(string type, string nodeName, List<string> Instructions, string rawContents)
        {
            this.NodeType = type;
            this.Contents = rawContents;
            this.Instructions = Instructions;
            allNodes[nodeName] = this;
            if (this.NodeType == "bank")
            {
                // * types
            }
            else if (this.NodeType == "tree")
            {
                // * types, inheritance
            }
            else if (this.NodeType == "neural")
            {
                // Special, limited types && methods for learning
            }
        }

        public void AddVariable(string varName, string varValue, string nodeName)
        {
            (allNodes[nodeName]).Variables[varName] = varValue; // This looks horrible, but it is adding a variable value with the key of the varName to the Variables dictionary that is paired with that node. The node is stored in an allNodes dict
        }

    }
}
