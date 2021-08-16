export interface Post {
    id : number;
    title : string;
    content : string;
    createdAt : string;
    modifiedAt : string;
    imageURL : string | null;
    userID : string;
    owner: string;
}
