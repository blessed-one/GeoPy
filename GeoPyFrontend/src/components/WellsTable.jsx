import React, {useEffect, useMemo, useState} from 'react';
import {
    Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Button, TextField, Box
} from '@mui/material';
import {axiosToBackend} from "../hooks/useAxios.js";
import WellFormModal from "./WellFormModal/WellFormModal.jsx";
import {toast} from "react-toastify";

const WellsTable = () => {
    const [wells, setWells] = useState([]);
    const [open, setOpen] = useState(false);
    const [editWell, setEditWell] = useState(null);
    const [search, setSearch] = useState('');
    const [sortAsc, setSortAsc] = useState(true);

    const fetchData = () => {
        axiosToBackend.get('/wells')
            .then(res => setWells(res.data))
            .catch(err => toast.error(err, {toastId: "Fetch wells error"}));
    };

    useEffect(fetchData, []);

    const handleDelete = async (id) => {
        if (window.confirm('Удалить скважину?')) {
            await axiosToBackend.delete(`/wells/${id}`).then(() =>
            {
                fetchData()
                toast.success("Well successfully deleted", {toastId: `DeleteError ${id}`})
            }).catch(e => {
                switch (e.status) {
                    case 404:
                        toast.error("404: Well not found", {toastId: `DeleteError ${id}`})
                        break;
                    case 500:
                        toast.error("505: Internal Server Error", {toastId: `DeleteError ${id}`})
                }
            } );
        }
    };
    const filteredAndSortedWells = useMemo(() => {
        let filtered = wells.filter(w =>
            w.wellNumber?.toLowerCase().includes(search.toLowerCase())
        );

        return filtered.sort((a, b) => {
            if (!a.fieldName || !b.fieldName) return 0;
            return sortAsc
                ? a.fieldName.localeCompare(b.fieldName)
                : b.fieldName.localeCompare(a.fieldName);
        });
    }, [wells, search, sortAsc]);

    return (
        <>
            <Box display="flex" gap={2} my={2}>
                <TextField
                    label="Поиск по номеру скважины"
                    variant="outlined"
                    value={search}
                    onChange={e => setSearch(e.target.value)}
                />
                <Button variant="outlined" onClick={() => setSortAsc(prev => !prev)}>
                    Сортировка по месторождению {sortAsc ? '↑' : '↓'}
                </Button>
                <Button variant="contained" onClick={() => { setEditWell(null); setOpen(true); }}>
                    ➕ Добавить скважину
                </Button>
            </Box>

            <TableContainer component={Paper} sx={{ marginTop: 2 }}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>ID</TableCell>
                            <TableCell>Номер</TableCell>
                            <TableCell>Наименование месторождения</TableCell>
                            <TableCell>Дебит</TableCell>
                            <TableCell>Давление</TableCell>
                            <TableCell>Дата замера</TableCell>
                            <TableCell>Действия</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {filteredAndSortedWells?.map(well => (
                            <TableRow key={well.wellId}>
                                <TableCell>{well.wellId}</TableCell>
                                <TableCell>{well.wellNumber}</TableCell>
                                <TableCell>{well.fieldName}</TableCell>
                                <TableCell>{well.debit}</TableCell>
                                <TableCell>{well.pressure}</TableCell>
                                <TableCell>{well.measurementDate}</TableCell>

                                <TableCell>
                                    <Button onClick={() => { setEditWell(well); setOpen(true); }}>✏️</Button>
                                    <Button color="error" onClick={() => handleDelete(well.wellId)}>🗑️</Button>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>

            <WellFormModal open={open} handleClose={() => setOpen(false)} refreshData={fetchData} editData={editWell} />
        </>
    );
};

export default WellsTable;
