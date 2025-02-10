using AnnotatedSentence;
using MorphologicalAnalysis;

namespace SemanticRoleLabeling.Sentence.Propbank
{
    public class TurkishSentenceAutoArgument : SentenceAutoArgument
    {
        /**
         * <summary> Given the sentence for which the predicate(s) were determined before, this method automatically assigns
         * semantic role labels to some/all words in the sentence. The method first finds the first predicate, then assuming
         * that the shallow parse tags were preassigned, assigns ÖZNE tagged words ARG0; NESNE tagged words ARG1. If the
         * verb is in passive form, ÖZNE tagged words are assigned as ARG1.</summary>
         * <param name="sentence">The sentence for which semantic roles will be determined automatically.</param>
         * <returns>If the method assigned at least one word a semantic role label, the method returns true; false otherwise.</returns>
         */
        public override bool AutoArgument(AnnotatedSentence.AnnotatedSentence sentence)
        {
            var modified = false;
            string predicateId = null;
            for (var i = 0; i < sentence.WordCount(); i++)
            {
                var word = (AnnotatedWord) sentence.GetWord(i);
                if (word.GetArgumentList() != null && word.GetArgumentList().ContainsPredicate())
                {
                    predicateId = word.GetSemantic();
                    break;
                }
            }

            if (predicateId != null)
            {
                for (var i = 0; i < sentence.WordCount(); i++)
                {
                    var word = (AnnotatedWord) sentence.GetWord(i);
                    if (word.GetArgumentList() == null)
                    {
                        if (word.GetShallowParse() != null && word.GetShallowParse().Equals("ÖZNE"))
                        {
                            if (word.GetParse() != null && word.GetParse().ContainsTag(MorphologicalTag.PASSIVE))
                            {
                                word.SetArgumentList("ARG1$" + predicateId);
                            }
                            else
                            {
                                word.SetArgumentList("ARG0$" + predicateId);
                            }

                            modified = true;
                        }
                        else
                        {
                            if (word.GetShallowParse() != null && word.GetShallowParse().Equals("NESNE"))
                            {
                                word.SetArgumentList("ARG1$" + predicateId);
                                modified = true;
                            }
                        }
                    }
                }
            }

            return modified;
        }
    }
}