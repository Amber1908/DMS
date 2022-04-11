import { useRef, useState } from "react";

const initialDndState = {
    draggedFrom: null,
    draggedTo: null,
    isDragging: false,
    originalOrder: [],
    updatedOrder: []
}

export const useDragAndDrop = (list, setList, containerArg = null) => {
    const [dragAndDrop, setDragAndDrop] = useState(initialDndState);
    const target = useRef(null);
    const startInfo = useRef({x: null, y: null});
    const container = useRef(containerArg);

    const onDragStart = (e) => {
        if (!target.current.classList.contains("handle")) {
            e.preventDefault();
            return;
        }

        const initialPosition = parseInt(e.currentTarget.dataset.position);
        setDragAndDrop({
            ...dragAndDrop,
            draggedFrom: initialPosition,
            isDragging: true,
            originalOrder: list
        });

        e.dataTransfer.setData("text/html", '');
    }

    const onDrag = (e) => {
        // console.log("onDrag");
        e.preventDefault();

        if (!dragAndDrop.isDragging) return;

        let offset =e.clientY - startInfo.current.y;
        let direction = offset >= 0 ? 1 : -1;
        let moveValue = 10;
        let moveOffset =  Math.abs(offset) > 100 ? moveValue * direction : 0;
        container.current.scrollTop += moveOffset;
    }

    const onDragEnd = () => {
        setDragAndDrop({
            ...dragAndDrop,
            draggedFrom: null,
            draggedTo: null,
            isDragging: false
        });
    }

    const onDragOver = (e) => {
        e.preventDefault();

        if (!dragAndDrop.isDragging) return;

        let newList = dragAndDrop.originalOrder;
        const draggedFrom = dragAndDrop.draggedFrom;
        const draggedTo = parseInt(e.currentTarget.dataset.position);
        const itemDragged = newList[draggedFrom];
        const remainingItems = newList.filter((item, index) => index !== draggedFrom);
        newList = [
            ...remainingItems.slice(0, draggedTo),
            itemDragged,
            ...remainingItems.slice(draggedTo)
        ];

        if (draggedTo !== dragAndDrop.draggedTo) {
            setDragAndDrop({
                ...dragAndDrop,
                updatedOrder: newList,
                draggedTo: draggedTo
            });
        }
    }

    const onDrop = () => {
        if (!dragAndDrop.isDragging) return;

        setList(dragAndDrop.updatedOrder);
        onDragEnd();
    }

    const onMouseDown = (e) => {
        target.current = e.target;
        console.log(e);
        startInfo.current = { x: e.clientX, y: e.clientY };
    }

    return { onDragStart, onDrag, onDrop, onDragOver, onDragEnd, onMouseDown, dragAndDrop, container };
}
