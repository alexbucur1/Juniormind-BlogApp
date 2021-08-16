import { Entity, Column, PrimaryGeneratedColumn, ManyToOne, JoinColumn } from 'typeorm';
import { User } from './user.entity';

@Entity("blogposts")
export class BlogPost {
    @PrimaryGeneratedColumn()
    id : number;

    @Column("text")
    title : string;

    @Column("text")
    content : string;

    @Column("datetime")
    createdAt : string;

    @Column("datetime")
    modifiedAt : string;

    @Column("text")
    imageURL : string | null;

    @ManyToOne(() => User, { eager: true, onDelete: "CASCADE", nullable: false})
    user : User;
}