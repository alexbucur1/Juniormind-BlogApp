import { Body, Controller, Post, Res, UseFilters } from '@nestjs/common';
import { Client } from '../../models/client';
import { AuthService } from '../../services/auth.service';
import { constants } from '../../constants';

@Controller('callback')
export class CallbackController {
    constructor(private authService: AuthService){
    }

    @Post()
    async callback(@Body() input: Client, @Res() res){
       if(await this.authService.setClient(input)) {
        return res.redirect(constants.url_angular + '/login-success');
       }
        return res.redirect(`/login?callBackUrl=${this.authService.client.callBackUrl}`);
    }
}
