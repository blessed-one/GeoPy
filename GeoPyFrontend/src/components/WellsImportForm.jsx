import React, { useState } from 'react';
import {axiosToBackend} from "../hooks/useAxios.js";
import {Button} from "./UI/Button/Button.jsx";

const WellsImportForm = () => {
    const [file, setFile] = useState(null);
    const [status, setStatus] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!file) return;

        const formData = new FormData();
        formData.append('file', file);

        try {
            const response = await axiosToBackend.post('/wells/import', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
            });
            const { wellsAdded, wellsUpdated } = response.data;
            setStatus(`✅ Импорт: добавлено ${wellsAdded}, обновлено ${wellsUpdated}. Обновите страницу`);
        } catch (error) {
            setStatus('❌ Ошибка при импорте');
            console.error(error);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <div style={{marginTop: '50px'}}>
                <input type="file" accept=".xlsx" onChange={e => setFile(e.target.files[0])}/>
                <Button text={"📥 Импортировать Excel"} type="submit"></Button>
                <p>{status}</p>
            </div>

        </form>
    );
};

export default WellsImportForm;
