import { User } from "../models/entities/user.entity";
import { blogPostEx } from "./blogPostExample";

export const postsArrayEx = [
    blogPostEx,
    {
      id: 2,
      title: "Title",
      content: "This is a basic content.",
      user: {
          id: "user",
          firstName: "User One",
          lastName: "Kenobi",
          email: "email@yahoo.com"
      },
      createdAt: "",
      modifiedAt: "",
      imageURL: ""
    },
    {
      id: 3,
      title: "Title",
      content: "This is a basic content.",
      user: {
        id: "user",
        firstName: "User One",
        lastName: "Kenobi",
        email: "email@yahoo.com"
    },
      createdAt: "",
      modifiedAt: "",
      imageURL: ""
    },
    {
      id: 4,
      title: "Title",
      content: "This is a basic content.",
      user: {
        id: "user",
        firstName: "User One",
        lastName: "Kenobi",
        email: "email@yahoo.com"
    },
      createdAt: "",
      modifiedAt: "",
      imageURL: ""
    }
  ];