import axios from 'axios';


const apiService = axios.create({
    baseURL: 'https://localhost:44343/api/', // Đặt URL cơ sở của API của bạn
    headers: {
        'Content-Type': 'application/json',
    },
});
apiService.interceptors.request.use(
    (config) => {
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);
export default apiService;