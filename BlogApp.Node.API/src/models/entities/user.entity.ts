import { Entity, Column, PrimaryGeneratedColumn, PrimaryColumn } from 'typeorm';

@Entity("aspnetusers")
export class User {
    @PrimaryColumn("varchar", {
        length: 20
      })
    id: string;

    @Column()
    firstName: string;

    @Column("text")
    lastName: string;

    @Column("text")
    email: string;
}