export interface QuestionMetadata {
    questionId: string;
    text: string;
    author: string;
    votes: number;
    isApproved: boolean;
}

export class Question {
    constructor(
        public question: QuestionMetadata,
        public didVote: boolean
    ) {}
}
