export interface QuestionMetadata {
    questionId: string;
    text: string;
    author: string;
    votes: number;
    isApproved: boolean;
}

export interface Question {
    question: QuestionMetadata;
    didVote: boolean;
}
