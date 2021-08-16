import { IsNotEmpty, MinLength } from "class-validator";

export class BlogPostDto {
    id : number;

    @IsNotEmpty()
    title : string;

    @IsNotEmpty()
    @MinLength(1)
    content : string;

    createdAt : string;

    modifiedAt : string | null;

    imageURL : string | null;

    userID : string | null;

    owner: string | null;
}