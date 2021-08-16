import { Module } from '@nestjs/common';
import { AuthModule } from '../auth/auth.module';
import { CommentsService } from '../services/comments.service';
import { CommentsController } from './comments.controller';

@Module({
    controllers: [CommentsController],
    providers: [
        CommentsService,
    ],
    imports:[AuthModule]
})
export class CommentsModule {}
