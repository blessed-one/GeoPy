import React, { useState } from 'react';
import {axiosToBackend} from "../hooks/useAxios.jsx";

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
            setStatus(`✅ Импорт: добавлено ${wellsAdded}, обновлено ${wellsUpdated}`);
        } catch (error) {
            setStatus('❌ Ошибка при импорте');
            console.error(error);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <input type="file" accept=".xlsx" onChange={e => setFile(e.target.files[0])} />
            <button type="submit">📥 Импортировать Excel</button>
            <p>{status}</p>
        </form>
    );
};

export default WellsImportForm;
