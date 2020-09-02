declare const Buffer: any;

//'throwErrorOnHttpRequestFail' parameter added as a workaround. 'unhandledrejection' event is not supported by all browsers yet.
export class ApiClient {
    static get<TResponse>(relativeUrl: string, throwErrorOnHttpRequestFail = true): Promise<TResponse> {
        return fetch(encodeURI(relativeUrl), { credentials: 'include' }).then<TResponse>((data) => {
            return handleFetchResponse(data, throwErrorOnHttpRequestFail);
        });
    }

    static post<TBody, TResponse>(relativeUrl: string, body: TBody, throwErrorOnHttpRequestFail: boolean = true): Promise<TResponse> {
        const jsonBody = JSON.stringify(body);
        return fetch(relativeUrl, {
            method: 'POST',
            body: jsonBody,
            headers: new Headers({
                'Content-Type': 'application/json; charset=utf-8',
                'Content-Length': new Buffer(jsonBody).byteLength.toString(),
            }),
            credentials: 'include',
        }).then<TResponse>((data) => {
            return handleFetchResponse(data, throwErrorOnHttpRequestFail);
        });
    }

    static put<TBody, TResponse>(relativeUrl: string, body: TBody, throwErrorOnHttpRequestFail: boolean = true): Promise<TResponse> {
        const jsonBody = JSON.stringify(body);
        return fetch(relativeUrl, {
            method: 'PUT',
            body: jsonBody,
            headers: new Headers({
                'Content-Type': 'application/json; charset=utf-8',
                'Content-Length': new Buffer(jsonBody).byteLength.toString(),
            }),
            credentials: 'include',
        }).then<TResponse>((data) => {
            return handleFetchResponse(data, throwErrorOnHttpRequestFail);
        });
    }

    static delete(relativeUrl: string, throwErrorOnHttpRequestFail: boolean = true) {
        return fetch(relativeUrl, {
            method: 'DELETE',
            credentials: 'include',
        }).then((response) => {
            if (!response.ok) {
                if (throwErrorOnHttpRequestFail) {
                    setTimeout(function () {
                        throw new HttpRequestFailError('Delete operation failed.', response);
                    });
                }

                Promise.reject(response);
            }
        });
    }

    // HTTP FILE UPLOAD
    static upload(url: string, file: File, onProgress: (progress: number) => void, throwErrorOnHttpRequestFail: boolean = true): Promise<string> {
        return new Promise((resolve, reject) => {
            let formData: FormData = new FormData(),
                xhr: XMLHttpRequest = new XMLHttpRequest();

            formData.append('file', file);

            xhr.onreadystatechange = () => {
                if (xhr.readyState === 4) {
                    if (xhr.status === 200) {
                        resolve(JSON.parse(xhr.response) as string);
                    } else {
                        throw new HttpRequestFailError('Upload failed. Status Code: ' + xhr.status);
                    }
                }
            };

            xhr.upload.onprogress = (event) => {
                const progress = Math.round((event.loaded / event.total) * 100);
                onProgress(progress);
            };

            xhr.open('POST', url, true);
            xhr.send(formData);
        });
    }
}

export function handleFetchResponse<TResponse>(response: Response, throwErrorOnHttpRequestFail: boolean): Promise<TResponse> {
    if (response.ok) {
        var contentType = response.headers.get('content-type');
        if (contentType && contentType.indexOf('application/json') !== -1) {
            return response.json();
        } else {
            //if response is not json and mostly empty
            return Promise.resolve({} as TResponse);
        }
    }

    if (response.status == 401) {
        location.replace('/auth/logoff');
    }

    //HttpRequestFailError is handled by GlobalErrorHandler
    if (throwErrorOnHttpRequestFail) {
        setTimeout(function () {
            throw new HttpRequestFailError('API request failed: ' + response.url, response);
        });
    }

    return Promise.reject(response);
}

export default class HttpRequestFailError extends Error {
    //In response we can send meaningfull error message which will be displayed to user by GlobalErrorHandler.
    //Message could be sent as just string. Or if you return object, add Message property with the message.
    response?: Response;

    //Message which will be shown in developer's console.
    message: string;

    constructor(message: string, response?: Response) {
        if (message) {
            super(message);
        }

        this.message = message;
        this.response = response;

        Object.setPrototypeOf(this, HttpRequestFailError.prototype);
    }
}