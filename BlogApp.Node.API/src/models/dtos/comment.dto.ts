import { IsNotEmpty, MinLength, MaxLength } from "class-validator";

export class CommentDto {
    id: number;

    userID: string | null;

    @IsNotEmpty()
    postID: number;

    @IsNotEmpty()
    @MinLength(1)
    @MaxLength(900)
    content: string;

    date: string | Date;

    parentID: number | null;

    userFullName: string | null;

    repliesCount: number | null;
}