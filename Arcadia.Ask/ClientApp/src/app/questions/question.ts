export interface QuestionMetadata {
    readonly questionId: string;
    readonly text: string;
    readonly author: string;
    readonly votes: number;
    readonly isApproved: boolean;
}

export interface Question {
    readonly question: QuestionMetadata;
    readonly didVote: boolean;
}
