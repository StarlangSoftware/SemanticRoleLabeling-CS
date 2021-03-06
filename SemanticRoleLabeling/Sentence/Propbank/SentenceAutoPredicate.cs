namespace SemanticRoleLabeling.Sentence.Propbank
{
    public abstract class SentenceAutoPredicate
    {
        /**
         * <summary> The method should set determine all predicates in the sentence.</summary>
         * <param name="sentence">The sentence for which predicates will be determined automatically.</param>
         */
        public abstract bool AutoPredicate(AnnotatedSentence.AnnotatedSentence sentence);
    }
}