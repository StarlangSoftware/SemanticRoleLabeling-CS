using System;
using System.Collections.Generic;
using AnnotatedSentence;
using AnnotatedTree;
using AnnotatedTree.Processor;
using AnnotatedTree.Processor.Condition;
using Dictionary.Dictionary;
using PropBank;

namespace SemanticRoleLabeling.ParseTree.Propbank
{
    public abstract class AutoArgument
    {
        protected ViewLayerType secondLanguage;
        protected abstract bool AutoDetectArgument(ParseNodeDrawable parseNode, ArgumentType argumentType);

        protected AutoArgument(ViewLayerType secondLanguage)
        {
            this.secondLanguage = secondLanguage;
        }

        /// <summary>
        /// Given the parse tree and the frame net, the method collects all leaf nodes and tries to set a propbank argument
        /// label to them. Specifically it tries all possible argument types one by one ARG0 first, then ARG1, then ARG2 etc.
        /// Each argument type has a special function to accept. The special function checks basically if there is a specific
        /// type of ancestor (specific to the argument, for example SUBJ for ARG0), or not.
        /// </summary>
        /// <param name="parseTree">Parse tree for semantic role labeling</param>
        /// <param name="frameset">Frame net used in labeling.</param>
        public void autoArgument(ParseTreeDrawable parseTree, Frameset frameset)
        {
            var nodeDrawableCollector =
                new NodeDrawableCollector((ParseNodeDrawable) parseTree.GetRoot(), new IsTransferable(secondLanguage));
            var leafList = nodeDrawableCollector.Collect();
            foreach (var parseNode in leafList)
            {
                if (parseNode.GetLayerData(ViewLayerType.PROPBANK) == null)
                {
                    foreach (ArgumentType argumentType in Enum.GetValues(typeof(ArgumentType)))
                    {
                        if (frameset.ContainsArgument(argumentType) && AutoDetectArgument(parseNode, argumentType))
                        {
                            parseNode.GetLayerInfo().SetLayerData(ViewLayerType.PROPBANK,
                                ArgumentTypeStatic.GetPropbankType(argumentType));
                        }
                    }

                    if (Word.IsPunctuation(parseNode.GetLayerData(secondLanguage)))
                    {
                        parseNode.GetLayerInfo().SetLayerData(ViewLayerType.PROPBANK, "NONE");
                    }
                }
            }

            parseTree.Save();
        }
    }
}