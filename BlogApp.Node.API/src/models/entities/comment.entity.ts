import { Entity, Column, PrimaryGeneratedColumn, ManyToOne } from 'typeorm';
import { User } from './user.entity';
import { BlogPost } from './blogpost.entity';

@Entity("comments")
export class Comment {
    @PrimaryGeneratedColumn()
    id: number;

    @ManyToOne(() => User, { eager: true, onDelete: "CASCADE", nullable: false})
    user: User;

    @ManyToOne(() => BlogPost, { eager: true, onDelete: "CASCADE", nullable: false})
    post: BlogPost;

    @Column({
        length: 900
    })
    content: string;

    @Column("datetime")
    date: string | Date;

    @Column()
    parentID: number;
}