export interface QuestionMetadata {
    readonly questionId: string;
    readonly text: string;
    readonly author: string;
    readonly votes: number;
    readonly isApproved: boolean;
}

export interface Question {
    readonly metadata: QuestionMetadata;
    readonly didVote: boolean;
}
