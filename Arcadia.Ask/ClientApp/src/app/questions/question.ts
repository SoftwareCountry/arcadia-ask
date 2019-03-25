export interface Question {
    questionId: string;
    text: string;
    author: string;
    votes: number;
    isApproved: boolean;
    didVote: boolean;
}

export class QuestionImpl implements Question {
    questionId: string;
    text: string;
    author: string;
    votes: number;
    isApproved: boolean;
    didVote: boolean;

    constructor(
        text: string,
        author: string,
        votes: number,
        isApproved: boolean,
        didVote: boolean
    ) {
        this.text = text;
        this.author = author;
        this.votes = votes;
        this.isApproved = isApproved;
        this.didVote = didVote;
    }
}
