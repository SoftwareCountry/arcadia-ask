export interface Question {
    questionId: string;
    text: string;
    author: string;
    votes: number;
    isApproved: boolean;
    didVote: boolean;
}

export class QuestionImpl implements Question {
    public questionId: string;
    public text: string;
    public author: string;
    public votes: number;
    public isApproved: boolean;
    public didVote: boolean;

    constructor(
        questionId: string,
        text: string,
        author: string,
        votes: number,
        isApproved: boolean,
        didVote: boolean
    ) {
        this.questionId = questionId;
        this.text = text;
        this.author = author;
        this.votes = votes;
        this.isApproved = isApproved;
        this.didVote = didVote;
    }
}
