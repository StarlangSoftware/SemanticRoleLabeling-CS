using System;
using AnnotatedSentence;
using AnnotatedTree;
using ParseTree;
using PropBank;

namespace SemanticRoleLabeling.ParseTree.Propbank
{
    public class TurkishAutoArgument : AutoArgument
    {
        /// <summary>
        /// Sets the language.
        /// </summary>
        public TurkishAutoArgument() : base(ViewLayerType.TURKISH_WORD)
        {
        }

        /// <summary>
        /// Checks all ancestors of the current parse node, until an ancestor has a tag of given name, or the ancestor is
        /// null. Returns the ancestor with the given tag, or null.
        /// </summary>
        /// <param name="parseNode">Parse node to start checking ancestors.</param>
        /// <param name="name">Tag to check.</param>
        /// <returns>The ancestor of the given parse node with the given tag, if such ancestor does not exist, returns null.</returns>
        private bool CheckAncestors(ParseNode parseNode, String name)
        {
            while (parseNode != null)
            {
                if (parseNode.GetData().GetName().Equals(name))
                {
                    return true;
                }

                parseNode = parseNode.GetParent();
            }

            return false;
        }

        /// <summary>
        /// Checks all ancestors of the current parse node, until an ancestor has a tag with the given, or the ancestor is
        /// null. Returns the ancestor with the tag having the given suffix, or null.
        /// </summary>
        /// <param name="parseNode">Parse node to start checking ancestors.</param>
        /// <param name="suffix">Suffix of the tag to check.</param>
        /// <returns>The ancestor of the given parse node with the tag having the given suffix, if such ancestor does not
        /// exist, returns null.</returns>
        private bool CheckAncestorsUntil(ParseNode parseNode, String suffix)
        {
            while (parseNode != null)
            {
                if (parseNode.GetData().GetName().Contains("-" + suffix))
                {
                    return true;
                }

                parseNode = parseNode.GetParent();
            }

            return false;
        }

        /// <summary>
        /// The method tries to set the argument of the given parse node to the given argument type automatically. If the
        /// argument type condition matches the parse node, it returns true, otherwise it returns false.
        /// </summary>
        /// <param name="parseNode">Parse node to check for semantic role.</param>
        /// <param name="argumentType">Semantic role to check.</param>
        /// <returns>True, if the argument type condition matches the parse node, false otherwise.</returns>
        protected override bool AutoDetectArgument(ParseNodeDrawable parseNode, ArgumentType argumentType)
        {
            ParseNode parent = parseNode.GetParent();
            switch (argumentType)
            {
                case ArgumentType.ARG0:
                    if (CheckAncestorsUntil(parent, "SBJ"))
                    {
                        return true;
                    }

                    break;
                case ArgumentType.ARG1:
                    if (CheckAncestorsUntil(parent, "OBJ"))
                    {
                        return true;
                    }

                    break;
                case ArgumentType.ARGMADV:
                    if (CheckAncestorsUntil(parent, "ADV"))
                    {
                        return true;
                    }

                    break;
                case ArgumentType.ARGMTMP:
                    if (CheckAncestorsUntil(parent, "TMP"))
                    {
                        return true;
                    }

                    break;
                case ArgumentType.ARGMMNR:
                    if (CheckAncestorsUntil(parent, "MNR"))
                    {
                        return true;
                    }

                    break;
                case ArgumentType.ARGMLOC:
                    if (CheckAncestorsUntil(parent, "LOC"))
                    {
                        return true;
                    }

                    break;
                case ArgumentType.ARGMDIR:
                    if (CheckAncestorsUntil(parent, "DIR"))
                    {
                        return true;
                    }

                    break;
                case ArgumentType.ARGMDIS:
                    if (CheckAncestors(parent, "CC"))
                    {
                        return true;
                    }

                    break;
                case ArgumentType.ARGMEXT:
                    if (CheckAncestorsUntil(parent, "EXT"))
                    {
                        return true;
                    }

                    break;
            }

            return false;
        }
    }
}