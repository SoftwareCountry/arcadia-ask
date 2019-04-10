namespace Arcadia.Ask.Questions
{
    using System;
    using System.Threading;

    public class DisplayedQuestion : IDisplayedQuestion
    {
        private Guid currentDisplayedQuestionId;
        private readonly ReaderWriterLock rwLock = new ReaderWriterLock();

        public Guid CurrentDisplayedQuestionId
        {
            get
            {
                this.rwLock.AcquireReaderLock(-1);

                Guid result;

                try
                {
                    result = this.currentDisplayedQuestionId;
                }
                finally
                {
                    this.rwLock.ReleaseReaderLock();
                }

                return result;
            }
            set
            {
                this.rwLock.AcquireWriterLock(-1);

                try
                {
                    this.currentDisplayedQuestionId = value;
                }
                finally
                {
                    this.rwLock.ReleaseWriterLock();
                }
            }
        }
    }
}