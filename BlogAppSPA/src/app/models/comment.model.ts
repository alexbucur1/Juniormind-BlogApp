export interface Comment {
    id: number;
    userID: string;
    postID: number;
    content: string;
    date: string | Date;
    parentID: number | null;
    userFullName: string;
    repliesCount: number;
}
