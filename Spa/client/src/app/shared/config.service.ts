import { Injectable } from '@angular/core';
 
@Injectable()
export class ConfigService {    

    constructor() {}

    get authApiURI() {
        return 'https://localhost:44357/api';
    }    
     
    get resourceApiURI() {
        return 'https://localhost:44330/api';
    }  
}