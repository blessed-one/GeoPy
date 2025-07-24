import React, { useEffect, useState } from 'react';
import {
    Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper
} from '@mui/material';
import {axiosToBackend} from "../hooks/useAxios.js";

const WellsTable = () => {
    const [wells, setWells] = useState([]);

    useEffect(() => {
        axiosToBackend.get('/wells')
            .then(response => setWells(response.data))
            .catch(error => console.error('Ошибка при получении данных:', error));
    }, []);

    return (
        <TableContainer component={Paper}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>ID</TableCell>
                        <TableCell>Номер скважины</TableCell>
                        <TableCell>Дебит</TableCell>
                        <TableCell>Давление</TableCell>
                        <TableCell>Дата измерения</TableCell>
                        <TableCell>Месторождение</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {wells.map(well => (
                        <TableRow key={well.wellId}>
                            <TableCell>{well.wellId}</TableCell>
                            <TableCell>{well.wellNumber}</TableCell>
                            <TableCell>{well.debit}</TableCell>
                            <TableCell>{well.pressure}</TableCell>
                            <TableCell>{well.measurementDate}</TableCell>
                            <TableCell>{well.fieldName}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};

export default WellsTable;
