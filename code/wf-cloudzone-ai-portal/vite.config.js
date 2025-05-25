import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

//export default defineConfig({
//    plugins: [react()],
//    server: {
//        fs: {
//            strict: false, // Allows serving files outside `src/`
//        },
//    },
//    build: {
//        rollupOptions: {
//            output: {
//                assetFileNames: "[name].[ext]" // Ensures JSON is properly served
//            }
//        }
//    }
//});

export default defineConfig({
    plugins: [react()],
    server: {
        middlewareMode: false, // Stop treating JSON as a route
        headers: {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods": "GET, POST",
            "Access-Control-Max-Age": "86400"
        }
    }
});

