import React from 'react';
import {Button} from './UI/Button/Button.jsx'
import {axiosToBackend} from "../hooks/useAxios.js";

const WellsExportButton = () => {
    const handleExport = async () => {
        try {
            const response = await axiosToBackend.get('/wells/export', {
                responseType: 'blob',
            });

            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', 'wells.xlsx');
            document.body.appendChild(link);
            link.click();
            link.remove();
        } catch (error) {
            console.error('Ошибка при экспорте:', error);
        }
    };

    return (
        <Button text={'📤 Экспортировать Excel'} onClick={handleExport}></Button>
    );
};

export default WellsExportButton;